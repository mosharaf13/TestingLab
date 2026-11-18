using System;
using TestingLab.Core.Waves;
using UnityEngine;

namespace TestingLab.Gameplay.Config
{
    /// <summary>
    /// Inspector-friendly wave data. Converted to Core's immutable WaveDefinition
    /// before the game uses it (designer data ≠ domain object).
    /// </summary>
    [Serializable]
    public sealed class WaveEntry
    {
        [Tooltip("Seconds after the level starts.")]
        [SerializeField, Min(0f)] private float startTime;
        [SerializeField, Min(0)] private int coins = 3;
        [SerializeField, Min(0)] private int enemies;

        public WaveEntry(float startTime, int coins, int enemies)
        {
            this.startTime = startTime;
            this.coins = coins;
            this.enemies = enemies;
        }

        public float StartTime => startTime;
        public int Coins => coins;
        public int Enemies => enemies;

        public WaveDefinition ToDefinition() => new WaveDefinition(startTime, coins, enemies);
    }
}
