using System;

namespace TestingLab.Core.Progression
{
    // Learning level: BEGINNER
    // The player's upgradeable stats. Shop items (IItemEffect) change these; the
    // Arena reads them when a level starts.

    public sealed class PlayerStats
    {
        public const int DefaultMaxHealth = 100;
        public const int DefaultArmor = 0;
        public const float DefaultMoveSpeed = 6f;

        public int MaxHealth { get; private set; }
        public int Armor { get; private set; }
        public float MoveSpeed { get; private set; }

        public PlayerStats(int maxHealth = DefaultMaxHealth, int armor = DefaultArmor, float moveSpeed = DefaultMoveSpeed)
        {
            if (maxHealth <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxHealth));
            if (armor < 0)
                throw new ArgumentOutOfRangeException(nameof(armor));
            if (moveSpeed <= 0f)
                throw new ArgumentOutOfRangeException(nameof(moveSpeed));

            MaxHealth = maxHealth;
            Armor = armor;
            MoveSpeed = moveSpeed;
        }

        public void IncreaseMaxHealth(int amount) => MaxHealth += RequirePositive(amount);

        public void IncreaseArmor(int amount) => Armor += RequirePositive(amount);

        public void IncreaseMoveSpeed(float amount)
        {
            if (amount <= 0f)
                throw new ArgumentOutOfRangeException(nameof(amount));
            MoveSpeed += amount;
        }

        private static int RequirePositive(int amount) =>
            amount > 0 ? amount : throw new ArgumentOutOfRangeException(nameof(amount));
    }
}
