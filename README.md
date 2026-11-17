# Testing Lab: a Unity project for learning automated testing

A small game-logic codebase with a full test suite, ready-made exercises and a bug-hunt
class. Students learn the Unity Test Framework (NUnit) on code that looks like a real game.

## Opening the project

1. In Unity Hub choose **Add > Add project from disk** and pick this folder.
2. Any Unity 6 editor works. If Hub says the version (6000.0.40f1) isn't installed,
   pick the Unity 6 version you have and confirm the upgrade.
3. Open **Window > General > Test Runner**. You'll see **EditMode** and **PlayMode** tabs.
4. Click **Run All** in each tab. Everything should be green except the exercises,
   which show as yellow (ignored) until students start them.
5. Optional: **Testing Lab > Build Demo Scene** in the menu bar creates a playable scene.
   Press Play and use WASD to collect coins.

## What's inside

```
Assets/
  Scripts/Runtime/         Game code (TestingLab.Runtime assembly)
    Health, Inventory          Pure C# logic: easy to test
    ScoreKeeper + ITimeProvider    Time injected so tests control the clock
    DamageCalculator + IRandomSource   Randomness injected so tests are repeatable
    ShopCart                   Bug hunt: has a written spec and some deliberate bugs
    HealthComponent, Mover, Collector, Collectible   MonoBehaviours
    DemoPlayerInput, Spinner   Demo-only, intentionally untested
  Scripts/Editor/          Menu item that builds the demo scene
  Tests/EditMode/          Fast tests, no Play mode (TestingLab.Tests.EditMode)
    TestDoubles/               FakeTimeProvider, FakeRandomSource
    Exercises/                 Student tasks, marked [Ignore] until started
  Tests/PlayMode/          Tests that need frames or physics (TestingLab.Tests.PlayMode)
.github/workflows/tests.yml    Runs all tests on GitHub (GameCI)
```

## Suggested lesson sequence

| # | Topic | Files |
|---|-------|-------|
| 1 | Why test? Test Runner tour, first test, red/green | `HealthTests` (first 2 tests) |
| 2 | Arrange-Act-Assert, `Assert.That`, exceptions, events | `HealthTests` |
| 3 | `[SetUp]`, `[TestCase]`, `ExpectedResult`, boundary values | `InventoryTests` |
| 4 | Test doubles: faking time and randomness | `ScoreKeeperTests`, `DamageCalculatorTests` |
| 5 | MonoBehaviours in Edit Mode, `LogAssert`, cleanup | `ComponentEditModeTests` |
| 6 | Play Mode, `[UnityTest]`, waiting for frames, timeouts | `MoverTests` |
| 7 | Physics tests, `WaitForFixedUpdate`, Unity's fake null | `CollectibleTests`, `HealthComponentPlayModeTests` |
| 8 | Bug hunt: test from a spec, then fix the code | `Exercises/ShopCartTests` |
| 9 | Continuous integration with GitHub Actions | `.github/workflows/tests.yml` |

Practice tasks for any point after lesson 4 are in `Exercises/MoreExercises.cs`.

## Key ideas the project demonstrates

- **Keep logic out of MonoBehaviours.** `Health` and `Inventory` are plain classes, so their
  tests run in milliseconds. `HealthComponent` and `Collector` are thin wrappers.
- **Inject what you can't control.** Time and randomness come in through interfaces, so tests
  replace them with fakes instead of waiting or hoping for a lucky roll.
- **Edit Mode vs Play Mode.** In Edit Mode, `Awake`/`Update` don't run and you must use
  `DestroyImmediate`. In Play Mode, they run for real and you `yield` to let frames pass.
- **Destroyed Unity objects aren't really null.** Use `obj == null`, not `Is.Null`.
- **Always add a timeout** to a `while` loop that waits in a `[UnityTest]`.

## Running tests from the command line

Useful for grading or scripting on the lab machines (Windows or Ubuntu):

```
Unity -batchmode -projectPath <path-to-TestingLab> -runTests -testPlatform EditMode -testResults results-edit.xml
Unity -batchmode -projectPath <path-to-TestingLab> -runTests -testPlatform PlayMode -testResults results-play.xml
```

Don't add `-quit`; `-runTests` exits by itself when the run finishes. The exit code is
non-zero if any test fails.

## GitHub Actions

Push the project to GitHub and the workflow runs every test on each commit. It needs a
Unity license stored as repository secrets (`UNITY_LICENSE`, `UNITY_EMAIL`, `UNITY_PASSWORD`);
GameCI's activation guide at https://game.ci/docs/github/activation explains how to get them.
Try it yourself before assigning it, since license activation is the step that usually trips
people up.
