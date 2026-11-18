using NUnit.Framework;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Exercises.Level3_Advanced
{
    // ═════════════════════════════════════════════════════════════════════════
    //  EXERCISES — Learning level: ADVANCED
    // ═════════════════════════════════════════════════════════════════════════

    [Category(TestCategory.Advanced), Category(TestCategory.Exercise)]
    public class AdvancedExercises
    {
        [Test, Ignore("Exercise A1 (spies): RunProgression.RetryLevel navigates to the Arena and does NOT save")]
        public void RunProgression_Retry_GoesToArenaWithoutSaving()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise A2 (content): every ShopCatalog item's DisplayName is unique (see ContentValidationTests)")]
        public void ShopCatalog_DisplayNamesAreUnique()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise A3 (property): GameSession.TimeRemaining never increases, for 1000 random Ticks/Pause/Resume")]
        public void GameSession_TimeNeverGoesBackwards()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise A4 (builder): add WithStats(...) to PlayerProfileBuilder and use it in a test")]
        public void Builder_Extension()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise A5 (TDD): write tests FIRST for a new Inventory.MoveTo(other, id, count), then implement it")]
        public void Inventory_MoveTo_TestDriven()
        {
            Assert.Fail("TODO");
        }
    }
}
