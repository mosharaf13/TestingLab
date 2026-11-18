using System;
using TestingLab.Core.Items;

namespace TestingLab.Core.Progression
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Tested by:      Tests/EditMode/Level2_Intermediate/PlayerProfileTests.cs
    //
    // Everything that survives between scenes: coins, stats, current level and
    // scores. It is saved by an IProfileStore (ProfileRepository in the game).
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class PlayerProfile
    {
        public PlayerStats Stats { get; }
        public Wallet Wallet { get; }

        /// <summary>Zero-based index of the level the player will play next.</summary>
        public int LevelIndex { get; private set; }

        /// <summary>Sum of the scores of every level WON in this run.</summary>
        public int RunScore { get; private set; }

        /// <summary>Best score ever reached. Survives starting a new run.</summary>
        public int BestScore { get; private set; }

        public PlayerProfile(PlayerStats stats, Wallet wallet, int levelIndex = 0, int runScore = 0, int bestScore = 0)
        {
            Stats = stats ?? throw new ArgumentNullException(nameof(stats));
            Wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
            if (levelIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(levelIndex));
            if (runScore < 0)
                throw new ArgumentOutOfRangeException(nameof(runScore));
            if (bestScore < 0)
                throw new ArgumentOutOfRangeException(nameof(bestScore));

            LevelIndex = levelIndex;
            RunScore = runScore;
            BestScore = Math.Max(bestScore, runScore);
        }

        /// <summary>A fresh run that keeps the all-time best score.</summary>
        public static PlayerProfile NewRun(int bestScore = 0) =>
            new PlayerProfile(new PlayerStats(), new Wallet(), bestScore: bestScore);

        /// <summary>
        /// Records the end of a level. Winning banks the coins and score and moves on
        /// to the next level. Losing keeps you on the same level (you can retry), but
        /// a great losing attempt can still set a new best score.
        /// </summary>
        public void CompleteLevel(bool won, int coinsCollected, int levelScore)
        {
            if (coinsCollected < 0)
                throw new ArgumentOutOfRangeException(nameof(coinsCollected));
            if (levelScore < 0)
                throw new ArgumentOutOfRangeException(nameof(levelScore));

            int attemptTotal = RunScore + levelScore;
            BestScore = Math.Max(BestScore, attemptTotal);

            if (!won)
                return;

            RunScore = attemptTotal;
            Wallet.Earn(coinsCollected);
            LevelIndex++;
        }
    }
}
