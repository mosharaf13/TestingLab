using System;

namespace TestingLab.Core.Common
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Tested by:      Tests/EditMode/Level2_Intermediate/CooldownTests.cs
    //
    // "Can I do this again yet?" Used for the player's invulnerability window after
    // being hit. Time comes from ITimeProvider, so tests don't have to wait.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class Cooldown
    {
        private readonly ITimeProvider _time;
        private float _readyAt = float.NegativeInfinity;

        public float DurationSeconds { get; }

        public bool IsReady => _time.Now >= _readyAt;

        public float RemainingSeconds => Math.Max(0f, _readyAt - _time.Now);

        public Cooldown(ITimeProvider time, float durationSeconds)
        {
            _time = time ?? throw new ArgumentNullException(nameof(time));
            if (durationSeconds < 0f)
                throw new ArgumentOutOfRangeException(nameof(durationSeconds), "Duration cannot be negative.");

            DurationSeconds = durationSeconds;
        }

        /// <summary>Starts the cooldown if it is ready.</summary>
        /// <returns>True if the action may happen now; false if still cooling down.</returns>
        public bool TryUse()
        {
            if (!IsReady)
                return false;

            _readyAt = _time.Now + DurationSeconds;
            return true;
        }

        public void Reset() => _readyAt = float.NegativeInfinity;
    }
}
