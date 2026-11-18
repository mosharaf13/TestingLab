# 🟢 Beginner: your first tests

**Goal:** by the end you can read and write a unit test, run it in Unity, and explain why a
failing test is good news.

## 1. Why test at all?

You changed the damage formula. Did you break the shop? The pause menu? Level 3?
Without tests, the only way to find out is to **play the whole game again**, every time you
change anything. An automated test is a tiny program that checks one thing in milliseconds.
A project with 200 tests re-checks 200 things every time you press *Run All*.

Tests also act as **living documentation**: `TimeFormatterTests` shows exactly how the timer
formats `59.2` seconds, more precisely than any comment could.

## 2. Running tests

*Window ▸ General ▸ Test Runner*. Two tabs:

- **EditMode**: runs without pressing Play. Fast. Most of our tests live here.
- **PlayMode**: enters Play mode, so `Update()`, physics and scenes work. Slower.

Click **Run All**. ✔ green = passed, ✘ red = failed (click it to read why), ⚠ yellow = ignored.
Use the **category** dropdown and choose `Beginner` to see only the tests for this guide.

## 3. Anatomy of a test

Open `Assets/Tests/EditMode/Level1_Beginner/StartHereTests.cs`:

```csharp
[Test]
public void Earn_AddsCoinsToTheBalance()
{
    // Arrange: set up what you need
    var wallet = new Wallet();

    // Act: do ONE thing
    wallet.Earn(5);

    // Assert: check the result
    Assert.That(wallet.Coins, Is.EqualTo(5));
}
```

- `[Test]` tells NUnit (the testing library) "this method is a test".
- The **name** says *what* happens *when* and *what should result*. When it fails, the name
  alone should tell you what broke.
- **Arrange, Act, Assert** (AAA). Keep the three parts visible. One *Act* per test.

## 4. Assertions you'll use most

```csharp
Assert.That(actual, Is.EqualTo(expected));
Assert.That(health.IsDead, Is.True);
Assert.That(score, Is.GreaterThan(0));
Assert.That(text, Does.Contain("3/10"));
Assert.That(list, Is.Empty);
Assert.That(0.1f + 0.2f, Is.EqualTo(0.3f).Within(0.0001f));   // floats: ALWAYS a tolerance
Assert.Throws<ArgumentOutOfRangeException>(() => health.TakeDamage(-5));
```

## 5. What should I test? Edge cases!

Most bugs hide at the **edges**, not in the middle. For any rule, test:

| Case | Example from `WalletTests` |
|------|----------------------------|
| A normal value | spend 4 of 10 → 6 left |
| Exactly on the boundary | spend 10 of 10 → 0 left |
| Just past the boundary | spend 11 of 10 → refused, still 10 |
| Zero / empty / null | spend 0 |
| Invalid input | spend -1 → throws |

## 6. `[SetUp]`: shared Arrange code

When every test starts the same way, move that code into a `[SetUp]` method
(`InventoryTests`). It runs **before each test**, so every test gets a *fresh* object.
Tests must never depend on each other or on the order they run in.

## 7. `[TestCase]`: one test, many examples

```csharp
[TestCase(60f, ExpectedResult = "1:00")]
[TestCase(9f,  ExpectedResult = "0:09")]
[TestCase(-5f, ExpectedResult = "0:00")]
public string MinutesSeconds_Formats(float seconds) => TimeFormatter.MinutesSeconds(seconds);
```

Each line shows up as its own test. Perfect for formatting, maths and conversion tables
(`TimeFormatterTests`, `HudFormatterTests`, `ResultsFormatterTests`).

## 8. Testing events

C# events (`Health.Died`, `Wallet.Changed`) are tested by subscribing a tiny lambda that
records what happened:

```csharp
int diedCount = 0;
health.Died += () => diedCount++;
health.TakeDamage(100);
health.TakeDamage(10);
Assert.That(diedCount, Is.EqualTo(1));
```

## 9. Why is this code so easy to test?

Look at `Health.cs`: it is **plain C#**, not a `MonoBehaviour`. You can `new` it in a test.
The Unity part (`HealthComponent`) is a thin shell around it. This is the single most
important design idea in the project; the [Intermediate guide](02-Intermediate.md) explains it.

## ✅ Practise

`Assets/Tests/EditMode/Exercises/Level1_Beginner/BeginnerExercises.cs`, exercises B1–B6.
For each one: remove `[Ignore(...)]`, write Arrange/Act/Assert, make it green. Then **break the
code on purpose** and check your test goes red. A test that can't fail proves nothing.

**Checklist before moving on**
- [ ] I can run a single test and a single category
- [ ] I can read a red failure message and find the line that failed
- [ ] I write AAA tests with descriptive names
- [ ] I test boundaries, not just "normal" values
- [ ] I know why floats need `.Within(...)`
