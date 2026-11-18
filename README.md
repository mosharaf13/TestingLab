# Testing Lab: learn automated testing by testing a real game

**Coin Rush** is a small but complete Unity game: a main menu, three arena levels with waves
of coins and chasing enemies, a combo score, a health bar, a shop with upgrades, and save data.
Around it is a full test suite, written to be read, and organised into three learning levels:

| Level | You learn | Where |
|-------|-----------|-------|
| 🟢 **Beginner** | What a test is, Arrange-Act-Assert, assertions, `[SetUp]`, `[TestCase]`, edge cases | `Tests/EditMode/Level1_Beginner` |
| 🟡 **Intermediate** | Test doubles (fakes, stubs, spies), dependency injection, state machines, Edit Mode vs Play Mode, physics and frames | `Tests/EditMode/Level2_Intermediate`, `Tests/PlayMode/Level2_Intermediate` |
| 🔴 **Advanced** | Mocks and interaction tests, scene/UI integration tests, content validation, contract tests, property-based tests, architecture tests | `Tests/EditMode/Level3_Advanced`, `Tests/PlayMode/Level3_Advanced` |

No testing knowledge is assumed. Start with the [Beginner guide](Docs/01-Beginner.md).

---

## Quick start

1. Open the project in **Unity 6000.3** (any Unity 6 should work).
2. **Play the game:** open `Assets/Scenes/MainMenu.unity` and press Play.
   WASD/arrows to move, collect coins, avoid red enemies, **Esc** to pause.
   Press **F1** in any scene for *Lab notes*: which tests cover what you're looking at.
3. **Run the tests:** *Window ▸ General ▸ Test Runner*. Click **Run All** in the **EditMode** tab,
   then in the **PlayMode** tab. Everything is green except the exercises, which are yellow
   (ignored) until a student starts them.
4. **Filter by level:** in the Test Runner, use the **category** dropdown to run only
   `Beginner`, `Intermediate`, `Advanced`, `Exercise` or `Integration` tests.

Useful menu items live under **Testing Lab** in the menu bar (open scenes, delete save data,
rebuild all scenes from code).

## The game, and the test that protects each part

| What you see in the game | Code | Kind of test | Level |
|---|---|---|---|
| Health bar, damage | `Core/Combat/Health` | Unit test | 🟢 |
| Countdown timer text | `Core/Presentation/TimeFormatter` | Unit test, `[TestCase]` table | 🟢 |
| Coins you carry to the shop | `Core/Items/Wallet` | Unit test | 🟢 |
| Combo multiplier x2…x4 | `Core/Scoring/ScoreKeeper` | Unit test with a **fake clock** | 🟡 |
| Critical hits, armor | `Core/Combat/DamageCalculator` | Unit test with **fake randomness** | 🟡 |
| Win / lose / pause | `Core/Session/GameSession` | **State machine** tests, `[TestCaseSource]` | 🟡 |
| Waves of coins and enemies | `Core/Waves/WaveSchedule` | Time-stepped unit tests | 🟡 |
| Enemy AI | `Gameplay/Enemies/ChaseBrain` | Unit test (decisions) + Play Mode test (movement) | 🟡 |
| Picking up coins | `Collector` + `Collectible` | **Play Mode** physics test | 🟡 |
| Player movement | `MovementMath` + `PlayerController` | Unit test + Play Mode test with **scripted input** | 🟡 |
| Saving progress | `ProfileRepository` | Unit test with **fake storage**, round-trip | 🟡 |
| Shop checkout | `Core/Shop/ShopService` | Unit test with **fakes and spies** | 🟡 |
| ⚠ Shop cart (has deliberate bugs!) | `Core/Shop/ShopCart` | **Bug hunt** exercise | 🟡 |
| Menu → Arena → Shop flow | `Core/Progression/RunProgression` | **Interaction tests** with spies | 🔴 |
| Clicking real buttons | Main menu, Arena, Shop scenes | **UI / integration** tests | 🔴 |
| Are levels even winnable? | `Assets/Data/Levels/*.asset` | **Content validation** tests | 🔴 |
| Forgot to drag a reference? | All scenes and prefabs | **Scene validation** tests | 🔴 |
| Fake save storage matches the real one | `IKeyValueStorage` | **Contract** tests | 🔴 |
| Rules that must *always* hold | Inventory, Wallet, ScoreKeeper, waves | **Property-based** tests | 🔴 |
| "Core never uses UnityEngine" | Assemblies | **Architecture** tests | 🔴 |

Not sure which kind of test to write? See [Which test should I write?](Docs/WhichTestShouldIWrite.md)

## Project layout

```
Assets/
  Scripts/
    Core/        TestingLab.Core: pure C# game rules. Can't reference UnityEngine at all.
    Gameplay/    TestingLab.Gameplay: MonoBehaviours, ScriptableObjects, UI views, scene controllers.
    Editor/      TestingLab.Editor: menu items and the code that builds the scenes.
  Tests/
    Shared/      Test doubles (Fakes/), Test Data Builders (Builders/), helpers (Support/).
    EditMode/    Fast tests, no Play mode.     Level1_Beginner / Level2_Intermediate / Level3_Advanced / Exercises
    PlayMode/    Tests that need frames, physics or scenes.   Level2_Intermediate / Level3_Advanced / Exercises
  Scenes/        MainMenu, Arena, Shop
  Data/          LevelCatalog, Levels/*, ShopCatalog (ScriptableObjects designers edit)
  Prefabs/       Coin, Enemy
Docs/            Learning guides, architecture, decision guide
```

Every source file starts with a header saying its **learning level**, the **design pattern** it
shows, and **which test file covers it**. Every test file says what it teaches.

## Learning path

| # | Topic | Read | Then try |
|---|-------|------|----------|
| 1 | What is a test? Test Runner tour, red/green | `Level1_Beginner/StartHereTests` | Break `Wallet.Earn`, watch it go red |
| 2 | Arrange-Act-Assert, exceptions, events | `HealthTests`, `WalletTests` | Exercises B1–B4 |
| 3 | `[SetUp]`, `[TestCase]`, boundary values | `InventoryTests`, `TimeFormatterTests` | Exercises B5–B6 |
| 4 | Test doubles: fake time and randomness | `Level2_Intermediate/ScoreKeeperTests`, `DamageCalculatorTests`, `CooldownTests` | Exercises I1–I4 |
| 5 | State machines, `[TestCaseSource]`, events | `GameSessionTests`, `WaveScheduleTests` | Exercises I5–I6 |
| 6 | Fakes + spies, "failure changes nothing" | `ShopServiceTests`, `ProfileRepositoryTests` | Exercise I7 |
| 7 | **Bug hunt:** test from a spec, then fix | `Exercises/Level2_Intermediate/ShopCartBugHuntTests` | Find all 3 bugs |
| 8 | MonoBehaviours in Edit Mode vs Play Mode | `ComponentEditModeTests`, `PlayMode/Level2_Intermediate/*` | Exercises P1–P3 |
| 9 | Interaction testing with spies | `Level3_Advanced/RunProgressionTests` | Exercise A1 |
| 10 | Scene and UI integration tests | `PlayMode/Level3_Advanced/*` | Exercises P4–P6 |
| 11 | Content, scene, contract, property and architecture tests | `EditMode/Level3_Advanced/*` | Exercises A2–A5 |
| 12 | Continuous integration | `.github/workflows/tests.yml` | Push and watch it run |

Guides: [Beginner](Docs/01-Beginner.md) · [Intermediate](Docs/02-Intermediate.md) ·
[Advanced](Docs/03-Advanced.md) · [Architecture & design patterns](Docs/Architecture.md)

## Running tests from the command line

Close the Editor first (a project can only be open once), then:

```
Unity -batchmode -projectPath <path-to-TestingLab> -runTests -testPlatform EditMode -testResults results-edit.xml
Unity -batchmode -projectPath <path-to-TestingLab> -runTests -testPlatform PlayMode -testResults results-play.xml
```

Add `-testCategory "Beginner"` to run one level only. Don't add `-quit`; `-runTests` exits by
itself. The exit code is non-zero if any test fails.

## GitHub Actions

`.github/workflows/tests.yml` runs every test on each push and pull request using GameCI.
It needs a Unity license stored as repository secrets (`UNITY_LICENSE`, `UNITY_EMAIL`,
`UNITY_PASSWORD`); see https://game.ci/docs/github/activation.
