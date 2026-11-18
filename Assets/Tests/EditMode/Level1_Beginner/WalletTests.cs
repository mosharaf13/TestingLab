using System;
using NUnit.Framework;
using TestingLab.Core.Items;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Level1_Beginner
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: BEGINNER
    // Code under test: Assets/Scripts/Core/Items/Wallet.cs
    // In the game:     coins you carry to the Shop.
    //
    // Topic: testing a RULE ("never below zero") from every direction:
    //        the normal case, the exact boundary, and one past the boundary.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Beginner)]
    public class WalletTests
    {
        [Test]
        public void TrySpend_WithEnoughCoins_PaysAndReturnsTrue()
        {
            var wallet = new Wallet(10);

            bool paid = wallet.TrySpend(4);

            Assert.That(paid, Is.True);
            Assert.That(wallet.Coins, Is.EqualTo(6));
        }

        [Test]
        public void TrySpend_ExactBalance_LeavesZero()
        {
            var wallet = new Wallet(10);

            bool paid = wallet.TrySpend(10);

            Assert.That(paid, Is.True);
            Assert.That(wallet.Coins, Is.EqualTo(0));
        }

        [Test]
        public void TrySpend_OneMoreThanBalance_FailsAndChangesNothing()
        {
            var wallet = new Wallet(10);

            bool paid = wallet.TrySpend(11);

            Assert.That(paid, Is.False);
            Assert.That(wallet.Coins, Is.EqualTo(10), "A failed purchase must not take any coins");
        }

        [Test]
        public void Earn_Negative_Throws()
        {
            var wallet = new Wallet();

            Assert.Throws<ArgumentOutOfRangeException>(() => wallet.Earn(-1));
        }

        [Test]
        public void Changed_ReportsNewBalance()
        {
            var wallet = new Wallet(5);
            int reported = -1;
            wallet.Changed += balance => reported = balance;

            wallet.Earn(3);

            Assert.That(reported, Is.EqualTo(8));
        }
    }
}
