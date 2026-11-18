using System;
using TestingLab.Core.Progression;

namespace TestingLab.Core.Shop
{
    public sealed class MaxHealthBoost : IItemEffect
    {
        private readonly int _amount;

        public MaxHealthBoost(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            _amount = amount;
        }

        public string Description => $"+{_amount} max health";

        public void Apply(PlayerStats stats) => stats.IncreaseMaxHealth(_amount);
    }
}
