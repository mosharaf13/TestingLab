using System;

namespace TestingLab
{
    /// <summary>
    /// Damage after armor: base * 100 / (100 + armor), rounded half away from zero.
    /// A critical hit multiplies the result. Any positive base damage deals at least 1.
    /// </summary>
    public class DamageCalculator
    {
        private readonly IRandomSource _random;

        public float CritChance { get; }
        public float CritMultiplier { get; }
        public bool LastHitWasCrit { get; private set; }

        public DamageCalculator(IRandomSource random, float critChance = 0.1f, float critMultiplier = 2f)
        {
            _random = random ?? throw new ArgumentNullException(nameof(random));
            if (critChance < 0f || critChance > 1f)
                throw new ArgumentOutOfRangeException(nameof(critChance));
            if (critMultiplier < 1f)
                throw new ArgumentOutOfRangeException(nameof(critMultiplier));

            CritChance = critChance;
            CritMultiplier = critMultiplier;
        }

        public int Calculate(int baseDamage, int armor)
        {
            if (baseDamage < 0)
                throw new ArgumentOutOfRangeException(nameof(baseDamage));
            if (armor < 0)
                throw new ArgumentOutOfRangeException(nameof(armor));

            double damage = baseDamage * (100.0 / (100.0 + armor));

            LastHitWasCrit = _random.Value01() < CritChance;
            if (LastHitWasCrit)
                damage *= CritMultiplier;

            int rounded = (int)Math.Round(damage, MidpointRounding.AwayFromZero);
            return baseDamage > 0 ? Math.Max(1, rounded) : 0;
        }
    }
}
