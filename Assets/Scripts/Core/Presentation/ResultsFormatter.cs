using TestingLab.Core.Session;

namespace TestingLab.Core.Presentation
{
    // Learning level: BEGINNER
    // Tested by: Tests/EditMode/Level1_Beginner/ResultsFormatterTests.cs
    // Text for the panel shown when a level ends.

    public static class ResultsFormatter
    {
        public static string Title(bool won, LoseReason reason)
        {
            if (won)
                return "Level Complete!";
            return reason == LoseReason.TimeUp ? "Time's Up!" : "Game Over";
        }

        public static string Summary(int levelScore, int coinsCollected, int bestScore) =>
            $"Score {levelScore}   Coins {coinsCollected}\nBest {bestScore}";

        public static string ContinueLabel(bool runComplete) => runComplete ? "Finish Run" : "To the Shop";
    }
}
