using System;
using TestingLab.Core.Items;
using TestingLab.Core.Progression;

namespace TestingLab.Gameplay.Infrastructure
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Pattern:        Data Transfer Object (DTO)
    //
    // JsonUtility can only save PUBLIC FIELDS. Our domain classes (PlayerProfile,
    // Wallet...) protect their data with private setters and validation, which is
    // good design but invisible to JsonUtility. So we copy the data into this
    // "dumb" DTO for saving, and back out again for loading.
    // The version field lets future builds migrate old save files.
    // ─────────────────────────────────────────────────────────────────────────

    [Serializable]
    public sealed class ProfileDto
    {
        public const int CurrentVersion = 1;

        public int version = CurrentVersion;
        public int levelIndex;
        public int coins;
        public int runScore;
        public int bestScore;
        public int maxHealth;
        public int armor;
        public float moveSpeed;

        public static ProfileDto FromProfile(PlayerProfile profile) => new ProfileDto
        {
            levelIndex = profile.LevelIndex,
            coins = profile.Wallet.Coins,
            runScore = profile.RunScore,
            bestScore = profile.BestScore,
            maxHealth = profile.Stats.MaxHealth,
            armor = profile.Stats.Armor,
            moveSpeed = profile.Stats.MoveSpeed
        };

        /// <exception cref="ArgumentException">If the saved data is invalid.</exception>
        public PlayerProfile ToProfile()
        {
            if (version != CurrentVersion)
                throw new ArgumentException($"Unsupported save version {version}.");

            var stats = new PlayerStats(maxHealth, armor, moveSpeed);
            return new PlayerProfile(stats, new Wallet(coins), levelIndex, runScore, bestScore);
        }
    }
}
