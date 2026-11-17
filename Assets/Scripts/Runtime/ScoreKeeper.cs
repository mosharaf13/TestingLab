using System;

namespace TestingLab
{
    /// <summary>
    /// Awards points with a combo multiplier. Hits within the combo window
    /// raise the multiplier (up to MaxMultiplier); a slower hit resets it to 1.
    /// </summary>
    public class ScoreKeeper
    {
        public const int MaxMultiplier = 4;

        private readonly ITimeProvider _time;
        private readonly float _comboWindow;
        private float _lastHitTime = float.NegativeInfinity;

        public int Score { get; private set; }
        public int Multiplier { get; private set; } = 1;

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
            Score += basePoints * Multiplier;
        }
    }
}
