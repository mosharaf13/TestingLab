namespace TestingLab.Core.Presentation
{
    // Learning level: BEGINNER
    // Tested by: Tests/EditMode/Level1_Beginner/HudFormatterTests.cs
    //
    // Pattern: Humble Object for UI. HudView (MonoBehaviour) only copies these
    // strings into Text components. All decisions about WHAT to show live here,
    // where a one-line test can check them.

    public static class HudFormatter
    {
        public static string Score(int score, int multiplier) =>
            multiplier > 1 ? $"Score {score}  x{multiplier}" : $"Score {score}";

        public static string Coins(int collected, int goal) => $"Coins {collected}/{goal}";

        public static string Health(int current, int max) => $"{current}/{max}";

        public static string Level(int levelIndex, int levelCount, string levelName) =>
            $"Level {levelIndex + 1}/{levelCount}: {levelName}";

        public static string Damage(int amount, bool critical) =>
            critical ? $"-{amount} CRIT!" : $"-{amount}";
    }
}
