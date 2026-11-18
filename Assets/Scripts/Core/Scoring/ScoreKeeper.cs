using System;
using TestingLab.Core.Common;

namespace TestingLab.Core.Scoring
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE  (faking time)
    // Tested by:      Tests/EditMode/Level2_Intermediate/ScoreKeeperTests.cs
    //
    // Awards points with a combo multiplier. Coins picked up within the combo
    // window raise the multiplier (up to MaxMultiplier); a slower pickup resets it.
    // In the Arena: grab coins quickly to see "x2", "x3", "x4" on the HUD.
    // ─────────────────────────────────────────────────────────────────────────

    public class ScoreKeeper
    {
        public const int MaxMultiplier = 4;

        private readonly ITimeProvider _time;
        private readonly float _comboWindow;
        private float _lastHitTime = float.NegativeInfinity;

        public int Score { get; private set; }
        public int Multiplier { get; private set; } = 1;

        /// <summary>Raised after points are added: (pointsAwarded, newMultiplier).</summary>
        public event Action<int, int> PointsAdded;

        public ScoreKeeper(ITimeProvider time, float comboWindowSeconds = 2f)
        {
            _time = time ?? throw new ArgumentNullException(nameof(time));
            if (comboWindowSeconds <= 0f)
                throw new ArgumentOutOfRangeException(nameof(comboWindowSeconds));
            _comboWindow = comboWindowSeconds;
        }

        public void AddPoints(int basePoints)
        {
            if (basePoints < 0)
                throw new ArgumentOutOfRangeException(nameof(basePoints));

            float now = _time.Now;
            if (now - _lastHitTime <= _comboWindow)
                Multiplier = Math.Min(MaxMultiplier, Multiplier + 1);
            else
                Multiplier = 1;

            _lastHitTime = now;
            int awarded = basePoints * Multiplier;
            Score += awarded;
            PointsAdded?.Invoke(awarded, Multiplier);
        }
    }
}
