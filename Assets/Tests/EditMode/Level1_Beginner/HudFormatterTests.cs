using NUnit.Framework;
using TestingLab.Core.Presentation;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Level1_Beginner
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: BEGINNER
    // Code under test: Assets/Scripts/Core/Presentation/HudFormatter.cs
    //
    // Why test text? Because the HUD is what players actually see. Keeping the
    // formatting out of the MonoBehaviour (HudView) makes it this easy to check.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Beginner)]
    public class HudFormatterTests
    {
        [Test]
        public void Score_WithoutCombo_HidesMultiplier()
        {
            Assert.That(HudFormatter.Score(120, 1), Is.EqualTo("Score 120"));
        }

        [Test]
        public void Score_WithCombo_ShowsMultiplier()
        {
            Assert.That(HudFormatter.Score(120, 3), Is.EqualTo("Score 120  x3"));
        }

        [Test]
        public void Coins_ShowsProgressTowardGoal()
        {
            Assert.That(HudFormatter.Coins(4, 10), Is.EqualTo("Coins 4/10"));
        }

        [Test]
        public void Level_IsOneBasedForPlayers()
        {
            // Code counts from 0, players count from 1. Easy off-by-one bug!
            Assert.That(HudFormatter.Level(0, 3, "Warm-up"), Is.EqualTo("Level 1/3: Warm-up"));
        }

        [TestCase(12, false, ExpectedResult = "-12")]
        [TestCase(24, true, ExpectedResult = "-24 CRIT!")]
        public string Damage_MarksCriticalHits(int amount, bool critical)
        {
            return HudFormatter.Damage(amount, critical);
        }
    }
}
