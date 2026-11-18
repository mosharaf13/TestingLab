using System.Collections.Generic;
using System.Linq;

namespace TestingLab.Core.Waves
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED  (content / data validation tests)
    // Tested by:      Tests/EditMode/Level2_Intermediate/LevelRulesTests.cs  (the rules)
    //                 Tests/EditMode/Level3_Advanced/LevelAssetValidationTests.cs
    //                 (runs these rules against EVERY LevelConfig asset in the project)
    //
    // Designers edit levels in the Inspector, not in code. A typo there (a coin goal
    // higher than the coins that ever spawn) makes a level impossible to win, and no
    // code test would notice. Content tests catch it before players do.
    // ─────────────────────────────────────────────────────────────────────────

    public static class LevelRules
    {
        /// <returns>Human-readable problems. Empty means the level is valid.</returns>
        public static IReadOnlyList<string> Validate(float timeLimitSeconds, int coinGoal, IReadOnlyList<WaveDefinition> waves)
        {
            var errors = new List<string>();

            if (timeLimitSeconds <= 0f)
                errors.Add($"Time limit must be positive (was {timeLimitSeconds}).");
            if (coinGoal <= 0)
                errors.Add($"Coin goal must be positive (was {coinGoal}).");

            if (waves == null || waves.Count == 0)
            {
                errors.Add("A level needs at least one wave.");
                return errors;
            }

            int totalCoins = waves.Sum(w => w.CoinCount);
            if (totalCoins < coinGoal)
                errors.Add($"Level can never be won: only {totalCoins} coins spawn but the goal is {coinGoal}.");

            foreach (var wave in waves.Where(w => w.StartTime >= timeLimitSeconds))
                errors.Add($"{wave} starts after the {timeLimitSeconds}s time limit, so it never appears.");

            return errors;
        }
    }
}
