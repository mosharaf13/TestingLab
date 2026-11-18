using System;
using TestingLab.Core.Progression;

namespace TestingLab.Core.Shop
{
    public sealed class SpeedBoost : IItemEffect
    {
        private readonly float _amount;

        public SpeedBoost(float amount)
        {
            if (amount <= 0f)
                throw new ArgumentOutOfRangeException(nameof(amount));
            _amount = amount;
        }

        public string Description => $"+{_amount:0.#} move speed";

        public void Apply(PlayerStats stats) => stats.IncreaseMoveSpeed(_amount);
    }
}
