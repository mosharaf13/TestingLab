using System;

namespace TestingLab
{
    /// <summary>
    /// Plain C# health model. No MonoBehaviour, so it can be tested fast in Edit Mode.
    /// </summary>
    public class Health
    {
        public int Max { get; }
        public int Current { get; private set; }
        public bool IsDead => Current <= 0;
        public float Percent => (float)Current / Max;

        /// <summary>Raised once, when health reaches zero.</summary>
        public event Action Died;

        /// <summary>Raised whenever health changes: (oldValue, newValue).</summary>
        public event Action<int, int> Changed;

        public Health(int max)
        {
            if (max <= 0)
                throw new ArgumentOutOfRangeException(nameof(max), "Max health must be positive.");

            Max = max;
            Current = max;
        }

        public void TakeDamage(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Damage cannot be negative. Use Heal instead.");
            if (IsDead || amount == 0)
                return;

            int old = Current;
            Current = Math.Max(0, Current - amount);
            Changed?.Invoke(old, Current);

            if (IsDead)
                Died?.Invoke();
        }

        public void Heal(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Heal amount cannot be negative.");
            if (IsDead || amount == 0)
                return; // the dead stay dead

            int old = Current;
            Current = Math.Min(Max, Current + amount);
            if (Current != old)
                Changed?.Invoke(old, Current);
        }
    }
}
