using NUnit.Framework;
using TestingLab.Core.Presentation;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Level1_Beginner
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: BEGINNER
    // Code under test: Assets/Scripts/Core/Presentation/TimeFormatter.cs
    // In the game:     the countdown timer, top-right of the Arena HUD.
    //
    // Topic: a TABLE of examples. Each [TestCase] line is one row:
    //        input → expected output. Great for formatting and maths.
    //        Notice how the table documents the behaviour better than prose.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Beginner)]
    public class TimeFormatterTests
    {
        [TestCase(60f, ExpectedResult = "1:00")]
        [TestCase(83f, ExpectedResult = "1:23")]
        [TestCase(9f, ExpectedResult = "0:09")]      // seconds are always two digits
        [TestCase(600f, ExpectedResult = "10:00")]
        [TestCase(59.2f, ExpectedResult = "1:00")]   // rounds UP...
        [TestCase(0.01f, ExpectedResult = "0:01")]   // ...so it only shows 0:00 at the very end
        [TestCase(0f, ExpectedResult = "0:00")]
        [TestCase(-5f, ExpectedResult = "0:00")]     // never shows a negative time
        public string MinutesSeconds_FormatsAsMinutesColonSeconds(float seconds)
        {
            return TimeFormatter.MinutesSeconds(seconds);
        }

        [Test]
        public void MinutesSeconds_NaN_ShowsZero()
        {
            Assert.That(TimeFormatter.MinutesSeconds(float.NaN), Is.EqualTo("0:00"));
        }
    }
}
