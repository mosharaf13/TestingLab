using System;
using TestingLab.Core.Progression;

namespace TestingLab.Core.Shop
{
    public sealed class ArmorBoost : IItemEffect
    {
        private readonly int _amount;

        public ArmorBoost(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            _amount = amount;
        }

        public string Description => $"+{_amount} armor";

        public void Apply(PlayerStats stats) => stats.IncreaseArmor(_amount);
    }
}
