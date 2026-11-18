# 🟡 Intermediate: test doubles, Unity components and Play Mode

**Goal:** test code that depends on time, randomness, storage, input and the Unity engine,
and know when you need Play Mode.

## 1. The problem: dependencies

`ScoreKeeper` gives a combo bonus if you pick up coins within 2 seconds. To test that "for
real" you would have to wait 2 seconds. `DamageCalculator` has a 10% critical-hit chance, so a
real-random test fails 1 run in 10. Code that reads `Time.time`, `Random.value`, `PlayerPrefs`
or the keyboard directly is **hard to test**.

## 2. The fix: inject an interface (Dependency Inversion)

```csharp
public interface ITimeProvider { float Now { get; } }

public class ScoreKeeper
{
    public ScoreKeeper(ITimeProvider time, float comboWindowSeconds = 2f) { ... }
}
```

The game passes `UnityTimeProvider` (reads `Time.time`). A test passes `FakeTimeProvider`
and moves time forward instantly:

```csharp
_clock = new FakeTimeProvider();
_score = new ScoreKeeper(_clock);
_score.AddPoints(10);
_clock.Advance(1f);          // "1 second later", in 0 ms
_score.AddPoints(10);
Assert.That(_score.Multiplier, Is.EqualTo(2));
```

This is **constructor injection**. The class says what it needs, and whoever creates it decides
which version to give it. In this project the "whoever" is a **composition root**
(`Gameplay/Infrastructure/GameServices.cs`, `ArenaController.Start`).

## 3. Kinds of test doubles

All of these live in `Assets/Tests/Shared/Fakes`.

| Double | What it does | Example |
|--------|--------------|---------|
| **Stub** | Returns canned answers | `FakeRandomSource`, `ScriptedPlayerInput` |
| **Fake** | A simple *working* implementation | `FakeTimeProvider`, `InMemoryKeyValueStorage`, `FakeProfileStore` |
| **Spy** | Records how it was used, so you can check afterwards | `SpyGameNavigator`, `SpyItemEffect`, `FakeRandomSource.CallCount` |
| **Mock** | A spy that is told up front what calls to expect (usually made with a library) | Not used, but see the Advanced guide |

## 4. Pure functions are the easiest code to test

`MovementMath.Step(position, input, speed, dt, bounds)` has no hidden state: the same inputs
always give the same output. We pulled it out of `PlayerController.Update()` on purpose.
**Rule of thumb:** move the decisions out of `Update()` into a plain method or class.

## 5. State machines (`GameSessionTests`)

`GameSession` moves Ready → Playing ⇄ Paused → Won/Lost. Test:
- every **legal** transition and the event it raises;
- every **illegal** move throws (Pause after losing, Start twice...);
- what is **ignored** in each state (coins don't count while paused).

`[TestCaseSource]` feeds a list of `(setup, illegalMove)` pairs, each shown as its own named test.

## 6. Edit Mode vs Play Mode

|                     | Edit Mode | Play Mode |
|---------------------|-----------|-----------|
| Speed               | ⚡ milliseconds | 🐢 frames and seconds |
| `Awake/Start/Update`| ❌ don't run (call setup yourself) | ✅ run |
| Physics, triggers   | ❌ | ✅ (wait for `WaitForFixedUpdate`) |
| `Destroy()`         | ❌ use `DestroyImmediate` | ✅ |
| Test attribute      | `[Test]` | `[UnityTest]` returning `IEnumerator` |

```csharp
[UnityTest]
public IEnumerator HoldingRight_MovesPlayerRight()
{
    _input.Move = Vector2.right;          // stub input instead of a keyboard
    yield return WaitFor.Frames(10);      // let Update() run 10 times
    Assert.That(_go.transform.position.x, Is.GreaterThan(0f));
}
```

Prefer Edit Mode. Use Play Mode only when you need frames, physics, coroutines or scenes.
Compare `ComponentEditModeTests` with `HealthComponentPlayModeTests` to see both styles.

## 7. Play Mode traps (we hit every one of these while building this project)

1. **Never wait without a timeout.** `while (!done) yield return null;` hangs the whole
   Test Runner if `done` never happens. Use `WaitFor.Condition(() => done, 2f, "message")`.
2. **Wait for time, not frames, when distance matters.** Frame length varies; an unfocused
   Editor can tick every millisecond. `PlayerControllerTests` uses `WaitForSeconds`.
3. **Physics stops when `Time.timeScale == 0`.** Waiting for `WaitForFixedUpdate` while the
   game is paused hangs forever. `WaitFor.Physics()` checks for this.
4. **Destroyed objects aren't `null` to NUnit.** Unity overrides `==`, so write
   `Assert.That(coin == null, Is.True)`, not `Is.Null`.
5. **Clean up.** Destroy what you create in `[TearDown]`, or the next test finds it.
6. **Floats add up.** `30 - 29.9f - 0.1f` is not `0`. Use values that are exact in binary
   (0.5, 0.25) or a tolerance (`GameSessionTests`).

## 8. Testing MonoBehaviours: method injection

MonoBehaviours have no constructors, so `DamageReceiver` takes its dependencies through a
`Construct(...)` method and falls back to real defaults if nobody calls it. Tests call
`Construct` with fakes. This mixes **real physics** with **fake time** in `ContactDamageTests`.

## 9. The Humble Object pattern

Look at `HealthComponent`, `EnemyController`, `HudView`, `SceneNavigator`. Each one is tiny
and has almost no logic; the logic lives in a plain class next to it (`Health`, `ChaseBrain`,
`HudFormatter`, `RunProgression`). The humble part is too simple to break, and the clever part
is easy to test.

## ✅ Practise

- `Exercises/Level2_Intermediate/IntermediateExercises.cs` (I1–I7)
- **Bug hunt:** `Exercises/Level2_Intermediate/ShopCartBugHuntTests.cs`. Play the Shop first:
  add the same item twice and try coupon `MEGA150`. Then write tests from the specification in
  `ShopCart.cs`, watch them fail, and fix the code.
- `Tests/PlayMode/Exercises/PlayModeExercises.cs` (P1–P3)

**Checklist**
- [ ] I can explain stub vs fake vs spy
- [ ] I can make time- or random-dependent code testable
- [ ] I know when a test must be Play Mode, and why I should avoid it otherwise
- [ ] Every wait in my `[UnityTest]`s has a timeout
