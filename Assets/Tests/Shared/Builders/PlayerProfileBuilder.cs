using TestingLab.Core.Items;
using TestingLab.Core.Progression;

namespace TestingLab.Tests.Shared.Builders
{
    /// <summary>Builds a PlayerProfile with sensible defaults. Override only what matters.</summary>
    public sealed class PlayerProfileBuilder
    {
        private int _coins;
        private int _levelIndex;
        private int _runScore;
        private int _bestScore;
        private int _maxHealth = PlayerStats.DefaultMaxHealth;
        private int _armor = PlayerStats.DefaultArmor;
        private float _moveSpeed = PlayerStats.DefaultMoveSpeed;

        public PlayerProfileBuilder WithCoins(int coins) { _coins = coins; return this; }
        public PlayerProfileBuilder AtLevel(int levelIndex) { _levelIndex = levelIndex; return this; }
        public PlayerProfileBuilder WithRunScore(int score) { _runScore = score; return this; }
        public PlayerProfileBuilder WithBestScore(int score) { _bestScore = score; return this; }
        public PlayerProfileBuilder WithMaxHealth(int maxHealth) { _maxHealth = maxHealth; return this; }
        public PlayerProfileBuilder WithArmor(int armor) { _armor = armor; return this; }
        public PlayerProfileBuilder WithMoveSpeed(float speed) { _moveSpeed = speed; return this; }

        public PlayerProfile Build() =>
            new PlayerProfile(new PlayerStats(_maxHealth, _armor, _moveSpeed), new Wallet(_coins), _levelIndex, _runScore, _bestScore);

        /// <summary>Lets you pass a builder wherever a PlayerProfile is expected.</summary>
        public static implicit operator PlayerProfile(PlayerProfileBuilder builder) => builder.Build();
    }
}
