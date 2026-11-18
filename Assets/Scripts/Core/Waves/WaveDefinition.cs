using System;

namespace TestingLab.Core.Waves
{
    /// <summary>One wave: at StartTime seconds, spawn some coins and some enemies.</summary>
    public readonly struct WaveDefinition
    {
        public float StartTime { get; }
        public int CoinCount { get; }
        public int EnemyCount { get; }

        public WaveDefinition(float startTime, int coinCount, int enemyCount)
        {
            if (startTime < 0f)
                throw new ArgumentOutOfRangeException(nameof(startTime));
            if (coinCount < 0)
                throw new ArgumentOutOfRangeException(nameof(coinCount));
            if (enemyCount < 0)
                throw new ArgumentOutOfRangeException(nameof(enemyCount));

            StartTime = startTime;
            CoinCount = coinCount;
            EnemyCount = enemyCount;
        }

        public override string ToString() => $"Wave@{StartTime:0.#}s ({CoinCount} coins, {EnemyCount} enemies)";
    }
}
