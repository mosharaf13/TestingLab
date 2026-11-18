using System;

namespace TestingLab.Core.Items
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: BEGINNER
    // Tested by:      Tests/EditMode/Level1_Beginner/WalletTests.cs
    //
    // The coins the player carries between levels and spends in the Shop.
    // Rule: the balance can never go below zero.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class Wallet
    {
        public int Coins { get; private set; }

        /// <summary>Raised with the new balance whenever it changes.</summary>
        public event Action<int> Changed;

        public Wallet(int startingCoins = 0)
        {
            if (startingCoins < 0)
                throw new ArgumentOutOfRangeException(nameof(startingCoins), "A wallet cannot start in debt.");

            Coins = startingCoins;
        }

        public void Earn(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Use TrySpend to remove coins.");
            if (amount == 0)
                return;

            Coins += amount;
            Changed?.Invoke(Coins);
        }

        /// <returns>True if paid; false (and no change) if there aren't enough coins.</returns>
        public bool TrySpend(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Cannot spend a negative amount.");
            if (amount > Coins)
                return false;
            if (amount == 0)
                return true;

            Coins -= amount;
            Changed?.Invoke(Coins);
            return true;
        }
    }
}
