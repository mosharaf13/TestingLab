using System;
using System.Collections.Generic;
using System.Linq;

namespace TestingLab.Core.Waves
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE  (time-stepped logic, testing events)
    // Tested by:      Tests/EditMode/Level2_Intermediate/WaveScheduleTests.cs
    //
    // Decides WHEN each wave starts. It does not spawn anything itself: WaveSpawner
    // (a MonoBehaviour) listens to WaveStarted and instantiates prefabs.
    // Splitting "when" from "how" means the timing rules are tested without a scene.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class WaveSchedule
    {
        private readonly List<WaveDefinition> _waves;
        private int _nextIndex;

        public float Elapsed { get; private set; }
        public int TotalWaves => _waves.Count;
        public int WavesStarted => _nextIndex;
        public bool IsFinished => _nextIndex >= _waves.Count;
        public int TotalCoins => _waves.Sum(w => w.CoinCount);

        /// <summary>Raised when a wave becomes due: (wave, zero-based index).</summary>
        public event Action<WaveDefinition, int> WaveStarted;

        public WaveSchedule(IEnumerable<WaveDefinition> waves)
        {
            if (waves == null)
                throw new ArgumentNullException(nameof(waves));

            // Stable sort: waves with equal start times keep the order they were given in.
            _waves = waves.OrderBy(w => w.StartTime).ToList();
        }

        /// <summary>Moves time forward and starts every wave that has become due.</summary>
        public void Advance(float deltaSeconds)
        {
            if (deltaSeconds < 0f)
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));

            Elapsed += deltaSeconds;
            while (_nextIndex < _waves.Count && _waves[_nextIndex].StartTime <= Elapsed)
            {
                int index = _nextIndex++;
                WaveStarted?.Invoke(_waves[index], index);
            }
        }
    }
}
