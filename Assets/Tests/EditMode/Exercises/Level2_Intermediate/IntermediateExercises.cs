using NUnit.Framework;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Exercises.Level2_Intermediate
{
    // ═════════════════════════════════════════════════════════════════════════
    //  EXERCISES — Learning level: INTERMEDIATE
    //  You'll need the fakes in Assets/Tests/Shared/Fakes (FakeTimeProvider,
    //  FakeRandomSource...). Look at ScoreKeeperTests for an example.
    // ═════════════════════════════════════════════════════════════════════════

    [Category(TestCategory.Intermediate), Category(TestCategory.Exercise)]
    public class IntermediateExercises
    {
        [Test, Ignore("Exercise I1: ScoreKeeper.AddPoints with a negative number throws ArgumentOutOfRangeException")]
        public void ScoreKeeper_NegativePoints_Throws()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise I2: a crit (fake roll 0.0) with armor 100 on 100 base damage deals 100")]
        public void DamageCalculator_CritWithArmor()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise I3: DamageCalculator rejects a crit chance above 1 or below 0 (use [TestCase])")]
        public void DamageCalculator_InvalidCritChance_Throws()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise I4: Cooldown.Reset() makes a cooling-down Cooldown ready immediately")]
        public void Cooldown_Reset_MakesItReady()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise I5: a Returning ChaseBrain that reaches home becomes Idle")]
        public void ChaseBrain_ReturningEnemyReachesHome_BecomesIdle()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise I6: GameSession ignores CollectCoins after the level is Won (coins stay at the goal)")]
        public void GameSession_CoinsAfterWinning_AreIgnored()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise I7: ShopService with a coupon that makes the cart free (100%) succeeds with 0 coins")]
        public void ShopService_FreeCart_SucceedsWithEmptyWallet()
        {
            Assert.Fail("TODO");
        }
    }
}
