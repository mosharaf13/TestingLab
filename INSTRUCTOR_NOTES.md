# Instructor notes (don't share with students before the bug hunt)

## Course map

| Week | Level | Topic | Student files | Exercises |
|------|-------|-------|---------------|-----------|
| 1 | 🟢 | Why test? Test Runner, AAA, assertions | `StartHereTests`, `HealthTests`, `WalletTests` | B1–B4 |
| 2 | 🟢 | `[SetUp]`, `[TestCase]`, boundaries, formatting tables | `InventoryTests`, `TimeFormatterTests`, `HudFormatterTests`, `CouponBookTests` | B5–B6 |
| 3 | 🟡 | Dependency injection & test doubles | `ScoreKeeperTests`, `DamageCalculatorTests`, `CooldownTests`, `SpawnPointPickerTests` | I1–I4 |
| 4 | 🟡 | State machines, events, `[TestCaseSource]` | `GameSessionTests`, `WaveScheduleTests`, `ChaseBrainTests` | I5–I6 |
| 5 | 🟡 | Fakes + spies, persistence, "failure changes nothing" | `ShopServiceTests`, `ProfileRepositoryTests`, `PlayerProfileTests` | I7 |
| 6 | 🟡 | **Bug hunt** (see below) | `ShopCartBugHuntTests` | all |
| 7 | 🟡 | Edit Mode vs Play Mode, physics, scripted input | `ComponentEditModeTests`, `PlayMode/Level2_Intermediate/*` | P1–P3 |
| 8 | 🔴 | Interaction testing, builders | `RunProgressionTests`, `Tests/Shared/Builders` | A1, A4 |
| 9 | 🔴 | Scene & UI tests, global state, flakiness | `PlayMode/Level3_Advanced/*` | P4–P6 |
| 10 | 🔴 | Content, contract, property and architecture tests; TDD; CI | `EditMode/Level3_Advanced/*`, `.github/workflows/tests.yml` | A2, A3, A5 |

Suggested demo for week 1: open the Arena, press F1 to show the lab notes, then break
`Health.TakeDamage` (remove the `Math.Max`) and run the Beginner category live.

## ShopCart deliberate bugs

All three are **visible in the game**, which makes a good hook: play the Shop first, then test.

| # | Spec rule | What actually happens | In-game symptom | Fix |
|---|-----------|-----------------------|-----------------|-----|
| 1 | Adding an existing SKU increases its quantity | The quantity is **overwritten** | Click "Add" twice → cart shows `1 x heart`, you pay once, get one upgrade | Add to the existing quantity |
| 2 | Removing an unknown SKU returns false | Throws **KeyNotFoundException** | (not reachable from the UI: a nice "why unit tests find bugs the UI hides" discussion) | `sku != null && _lines.Remove(sku)` |
| 3 | Coupon above 100 throws | Accepted, so **Total goes negative** | Coupon `MEGA150` → negative TOTAL; Checkout throws `ArgumentOutOfRangeException` from `Wallet.TrySpend` in the Console | Also throw when `percent > 100` |

Bug 3 is found twice: by `ApplyCoupon_Over100_Throws` and by `Total_IsNeverNegative`. A good
moment to discuss one bug causing several test failures. After bug 3 is fixed,
`ShopController` already catches the exception and shows *"Coupon "MEGA150" is not valid."*

**Discussion point, not a bug:** `Total` uses integer division, so 15% off 999 gives a discount
of 149 (not 149.85). The spec doesn't say how to round. Should a test pin this down, and who
decides? (It's a requirements question, not a code question.)

**Story point:** the coupon list (`CouponBook.CreateDefault`) contains `MEGA150` because "a
designer typed 150 instead of 15". Ask: which test should have caught *that*? (A content test
on the coupon list, or the cart's validation, ideally both.)

### Sample solutions

```csharp
[Test]
public void AddItem_SameSkuTwice_QuantitiesAreSummed()
{
    _cart.AddItem("potion", price: 20, quantity: 2);
    _cart.AddItem("potion", price: 20, quantity: 3);
    Assert.That(_cart.ItemCount, Is.EqualTo(5));               // fails: 3 (bug 1)
}

[Test]
public void RemoveItem_UnknownSku_ReturnsFalse()
{
    Assert.That(_cart.RemoveItem("nothing"), Is.False);        // fails: exception (bug 2)
}

[Test]
public void ApplyCoupon_Over100_Throws()
{
    Assert.Throws<ArgumentOutOfRangeException>(() => _cart.ApplyCoupon(101)); // fails (bug 3)
}

[Test]
public void ApplyCoupon_25Percent_ReducesTotal()
{
    _cart.AddItem("shield", price: 20);
    _cart.ApplyCoupon(25);
    Assert.That(_cart.Total, Is.EqualTo(15));                  // passes
}

[Test]
public void Total_IsNeverNegative()
{
    _cart.AddItem("shield", price: 20);
    _cart.ApplyCoupon(150);   // once bug 3 is fixed this throws; wrap or rethink the test
    Assert.That(_cart.Total, Is.GreaterThanOrEqualTo(0));      // fails (bug 3)
}

[Test]
public void Clear_RemovesItemsAndCoupon()
{
    _cart.AddItem("shield", price: 20);
    _cart.ApplyCoupon(10);
    _cart.Clear();
    Assert.That(_cart.ItemCount, Is.EqualTo(0));
    Assert.That(_cart.CouponPercent, Is.EqualTo(0));           // passes: not every test finds a bug
}
```

After fixing bug 3, `Total_IsNeverNegative` *errors* (the setup throws) instead of failing on its
assertion. A nice prompt for "tests change when the design changes".

Fixed `ShopCart` methods:

```csharp
public void AddItem(string sku, int price, int quantity = 1)
{
    // ...existing validation...
    _lines[sku] = new CartLine(sku, price, QuantityOf(sku) + quantity);
}

public bool RemoveItem(string sku) => sku != null && _lines.Remove(sku);

public void ApplyCoupon(int percent)
{
    if (percent < 0 || percent > 100)
        throw new ArgumentOutOfRangeException(nameof(percent));
    CouponPercent = percent;
}
```

## Exercise answer key (brief)

**Beginner**
- B1: `new Wallet(10).TrySpend(0)` → `true`, still 10.
- B2: subscribe a counter to `Changed`; `new Wallet(5).TrySpend(10)` → counter stays 0.
- B3: add 2, remove 5 → `false`, count still 2.
- B4: counter on `Changed`, `Heal(10)` at full health → 0 calls.
- B5: `[TestCase(61f, ExpectedResult = "1:01")]`, `(3599f, "59:59")`, `(3600f, "60:00")`.
- B6: `Assert.That(HudFormatter.Health(35, 100), Is.EqualTo("35/100"))`.

**Intermediate**
- I1: `Assert.Throws<ArgumentOutOfRangeException>(() => score.AddPoints(-1))`.
- I2: fake roll `0f`, base 100, armor 100 → 50 × 2 = **100**.
- I3: `[TestCase(1.5f)] [TestCase(-0.1f)]` on `new DamageCalculator(random, critChance)`.
- I4: `TryUse()`, then `Reset()` → `IsReady` is true with the clock unchanged.
- I5: chase (player at 4), player escapes (20) → `Returning`; call again with `self == home` and the player still far → `Idle`.
- I6: `Start()`, `CollectCoins(goal)` → Won; `CollectCoins(1)` → `CoinsCollected` still `goal`.
- I7: `AddItem("armor", 40)`, `ApplyCoupon(100)`, profile with 0 coins → `Success`, effect applied once.

**Advanced**
- A1: `RetryLevel()` → `_navigator.Calls` equals `{ Arena }` and `_store.SaveCount == 0`.
- A2: like `ShopCatalog_SkusAreUnique`, grouping by `DisplayName`.
- A3: seeded loop of random `Tick`, `TogglePause`; assert `TimeRemaining` never increases.
- A4: add `WithStats(int maxHealth, int armor, float speed)` returning `this`.
- A5: open-ended TDD. Look for tests of: partial move, not enough items, target full (nothing changes on either side).

**Play Mode**
- P1: `MoveTo(far)`, a few frames, `Stop()`, more frames → `IsMoving` false, `Arrived` never raised, position unchanged after the stop.
- P2: `MoveTo(A)`, a frame, `MoveTo(B)` → arrives at B.
- P3: like `ContactDamageTests` with `receiver.GodMode = true` → health stays 100.
- P4: `LoadArena()` (GodMode is on), `Time.timeScale = 20`, `WaitFor.Condition(() => Session.Phase == Lost, 10f, ...)`, title `"Time's Up!"`.
- P5 (after fixing bug 1): 30 coins, click `Add_heart` twice, Checkout → saved MaxHealth 150, coins 14.
- P6: `SaveData.Save(A.Profile().AtLevel(3))`, load MainMenu → `MainMenuView.MessageText` contains `"cleared every level"`, Continue hidden.

## Grading from the command line

Students remove `[Ignore]` as they go, so the `Exercise` category shows their progress:

```
Unity -batchmode -projectPath <student-project> -runTests -testPlatform EditMode -testCategory Exercise -testResults exercises.xml
```

Also run the whole suite: a student who "fixed" a test by breaking the game will turn
something else red.

## Teaching moments that happened while building this project

These are real bugs the tests caught during development. Each one makes a good short story:

1. **Float arithmetic.** A `GameSession` test ticked 29.9 s then 0.1 s and expected the time to
   be up. `30 - 29.9f - 0.1f` is `0.0000004`, so the level never ended. The fix was in the
   *test* (use exact binary values), and it shows why tolerances matter.
2. **Stale asset references.** The scene builder assigned ScriptableObjects that creating a new
   scene had just unloaded, so every scene saved "None". `SceneSetupValidationTests` caught it
   immediately. Without it, the first symptom would have been a crash on Play.
3. **A hung Play Mode test.** Collecting the winning coin sets `Time.timeScale = 0`, physics
   stops, and `WaitForFixedUpdate` never returns. The run hung for 10 minutes. Fixes:
   `WaitFor.Physics()` checks `timeScale`, and scene test classes have `[Timeout]`.
4. **Frame-count assumptions.** "Wait 10 frames" wasn't enough movement when the Editor ran
   very short frames. Waiting for *time* fixed it.

## Setup notes

- Built and verified on Unity **6000.3.24f1**, Built-in render pipeline, legacy Input Manager,
  uGUI (`com.unity.ugui`). Results: EditMode 200 passed / 24 ignored, PlayMode 26 passed /
  6 ignored (the ignored ones are the exercises).
- `com.unity.pipeline` in `Packages/manifest.json` is a development tool that let an AI
  assistant drive the Editor (build scenes, run tests). Students don't need it; remove it with
  the Package Manager before distributing if you prefer.
- Scenes/prefabs/data were generated by `Assets/Scripts/Editor/Builders`. **Testing Lab ▸
  Rebuild Game Content** regenerates them (overwriting edits). **Testing Lab ▸ Delete Save Data**
  resets progress.

## Ideas for extension

- **TDD a feature:** a "magnet" shop item that increases pickup radius. Test the effect first.
- **Mutation testing by hand:** change `<=` to `<` in `ScoreKeeper`, or remove the clamp in
  `MovementMath`. Which tests catch it?
- **New level:** students add `Level4.asset` and see a new test appear in `ContentValidationTests`.
- **Code coverage:** add `com.unity.testtools.codecoverage` and discuss why 100% isn't the goal.
- **Performance tests:** add `com.unity.test-framework.performance` and measure `WaveSchedule.Advance`.
