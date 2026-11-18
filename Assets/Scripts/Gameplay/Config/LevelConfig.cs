using System.Collections.Generic;
using System.Linq;
using TestingLab.Core.Waves;
using UnityEngine;

namespace TestingLab.Gameplay.Config
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE (data-driven design) / ADVANCED (asset tests)
    // Tested by:      Tests/EditMode/Level3_Advanced/LevelAssetValidationTests.cs
    // Pattern:        ScriptableObject configuration
    //
    // One asset per level (Assets/Data/Levels). Designers tune numbers here without
    // touching code. Create more via  Assets ▸ Create ▸ Testing Lab ▸ Level Config.
    // ─────────────────────────────────────────────────────────────────────────

    [CreateAssetMenu(menuName = "Testing Lab/Level Config", fileName = "Level")]
    public sealed class LevelConfig : ScriptableObject
    {
        [SerializeField] private string displayName = "New Level";
        [SerializeField, Min(1f)] private float timeLimitSeconds = 60f;
        [SerializeField, Min(1)] private int coinGoal = 10;
        [SerializeField, Min(0)] private int pointsPerCoin = 10;

        [Header("Enemies")]
        [SerializeField, Min(0f)] private float enemySpeed = 2.5f;
        [SerializeField, Min(0)] private int enemyContactDamage = 10;

        [Header("Waves")]
        [SerializeField] private List<WaveEntry> waves = new List<WaveEntry>();

        public string DisplayName => displayName;
        public float TimeLimitSeconds => timeLimitSeconds;
        public int CoinGoal => coinGoal;
        public int PointsPerCoin => pointsPerCoin;
        public float EnemySpeed => enemySpeed;
        public int EnemyContactDamage => enemyContactDamage;
        public IReadOnlyList<WaveEntry> Waves => waves;

        public IReadOnlyList<WaveDefinition> BuildWaves() => waves.Select(w => w.ToDefinition()).ToList();

        /// <returns>Design problems with this level. Empty means OK.</returns>
        public IReadOnlyList<string> Validate() => LevelRules.Validate(timeLimitSeconds, coinGoal, BuildWaves());

        /// <summary>Sets every field at once. Used by the scene builder and by tests.</summary>
        public void Configure(string levelName, float timeLimit, int goal, int coinPoints,
            float enemyMoveSpeed, int enemyDamage, IEnumerable<WaveEntry> waveEntries)
        {
            displayName = levelName;
            timeLimitSeconds = timeLimit;
            coinGoal = goal;
            pointsPerCoin = coinPoints;
            enemySpeed = enemyMoveSpeed;
            enemyContactDamage = enemyDamage;
            waves = waveEntries.ToList();
        }
    }
}
