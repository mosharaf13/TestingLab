using NUnit.Framework;
using TestingLab.Core.Presentation;
using TestingLab.Core.Session;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Level1_Beginner
{
    // Learning level: BEGINNER
    // Code under test: Assets/Scripts/Core/Presentation/ResultsFormatter.cs
    // In the game: the panel that appears when a level ends.

    [Category(TestCategory.Beginner)]
    public class ResultsFormatterTests
    {
        [TestCase(true, LoseReason.None, ExpectedResult = "Level Complete!")]
        [TestCase(false, LoseReason.TimeUp, ExpectedResult = "Time's Up!")]
        [TestCase(false, LoseReason.PlayerDied, ExpectedResult = "Game Over")]
        public string Title_DependsOnHowTheLevelEnded(bool won, LoseReason reason)
        {
            return ResultsFormatter.Title(won, reason);
        }

        [TestCase(false, ExpectedResult = "To the Shop")]
        [TestCase(true, ExpectedResult = "Finish Run")]
        public string ContinueLabel_AfterLastLevel_SaysFinish(bool runComplete)
        {
            return ResultsFormatter.ContinueLabel(runComplete);
        }

        [Test]
        public void Summary_ContainsScoreCoinsAndBest()
        {
            string summary = ResultsFormatter.Summary(levelScore: 150, coinsCollected: 12, bestScore: 900);

            Assert.That(summary, Does.Contain("150"));
            Assert.That(summary, Does.Contain("12"));
            Assert.That(summary, Does.Contain("900"));
        }
    }
}
