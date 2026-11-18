using NUnit.Framework;
using TestingLab.Core.Items;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Level1_Beginner
{
    // ═════════════════════════════════════════════════════════════════════════
    //  START HERE  —  Learning level: BEGINNER
    // ═════════════════════════════════════════════════════════════════════════
    //
    //  An automated test is a small method that runs a piece of game code and
    //  CHECKS the result. If the check fails, the test turns red and tells you
    //  exactly what went wrong. You never have to replay the game to find out.
    //
    //  HOW TO RUN:  Window ▸ General ▸ Test Runner ▸ EditMode tab ▸ Run All.
    //  Green ✔ = passed. Red ✘ = failed. Yellow ⚠ = ignored (exercises).
    //
    //  ANATOMY OF A TEST
    //    [Test]                        ← tells NUnit "this method is a test"
    //    public void What_When_Then()  ← name says what it checks
    //    {
    //        // Arrange: create the objects you need
    //        // Act:     do ONE thing
    //        // Assert:  check the result
    //    }
    //
    //  The [Category] on the class lets you filter by level in the Test Runner.
    //  Try it: break Wallet.Earn on purpose (e.g. Coins -= amount), re-run, read
    //  the red message, then undo your change.
    // ═════════════════════════════════════════════════════════════════════════

    [Category(TestCategory.Beginner)]
    public class StartHereTests
    {
        [Test]
        public void NewWallet_IsEmpty()
        {
            // Arrange + Act
            var wallet = new Wallet();

            // Assert: "I expect wallet.Coins to be equal to 0"
            Assert.That(wallet.Coins, Is.EqualTo(0));
        }

        [Test]
        public void Earn_AddsCoinsToTheBalance()
        {
            // Arrange
            var wallet = new Wallet();

            // Act
            wallet.Earn(5);

            // Assert
            Assert.That(wallet.Coins, Is.EqualTo(5));
        }

        [Test]
        public void AssertionVocabulary_Examples()
        {
            // A tour of the most useful constraints. Read the failure message each
            // one would give by changing an expected value and re-running.
            var wallet = new Wallet(10);

            Assert.That(wallet.Coins, Is.EqualTo(10));          // equality
            Assert.That(wallet.Coins, Is.GreaterThan(0));       // comparison
            Assert.That(wallet.TrySpend(3), Is.True);           // booleans
            Assert.That("Coins 3/10", Does.Contain("3/10"));    // strings
            Assert.That(new[] { 1, 2, 3 }, Has.Member(2));      // collections
            Assert.That(0.1f + 0.2f, Is.EqualTo(0.3f).Within(0.0001f)); // floats: ALWAYS use a tolerance
        }
    }
}
