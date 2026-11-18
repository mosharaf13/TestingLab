using NUnit.Framework;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Exercises.Level1_Beginner
{
    // ═════════════════════════════════════════════════════════════════════════
    //  EXERCISES — Learning level: BEGINNER
    //  Prerequisite: read Level1_Beginner/StartHereTests.cs and HealthTests.cs.
    //
    //  For each test: delete the [Ignore(...)] part, replace Assert.Fail with
    //  Arrange / Act / Assert, run it, and make it green. Then BREAK the code under
    //  test on purpose and check that your test turns red. A test that can't fail
    //  is worthless.
    // ═════════════════════════════════════════════════════════════════════════

    [Category(TestCategory.Beginner), Category(TestCategory.Exercise)]
    public class BeginnerExercises
    {
        [Test, Ignore("Exercise B1: Wallet.TrySpend(0) returns true and leaves the balance unchanged")]
        public void Wallet_SpendZero_IsAllowed()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise B2: Wallet.Changed is NOT raised when TrySpend fails (hint: count the calls)")]
        public void Wallet_FailedSpend_DoesNotRaiseChanged()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise B3: Inventory.Remove with more than you have returns false and changes nothing")]
        public void Inventory_RemoveMoreThanHeld_ReturnsFalse()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise B4: Health.Heal raises Changed, but NOT when already at full health")]
        public void Health_HealAtFullHealth_DoesNotRaiseChanged()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise B5: turn this into a [TestCase] table: 61s → \"1:01\", 3599s → \"59:59\", 3600s → \"60:00\"")]
        public void TimeFormatter_MoreExamples()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise B6: HudFormatter.Health(35, 100) returns \"35/100\"")]
        public void HudFormatter_Health()
        {
            Assert.Fail("TODO");
        }
    }
}
