using NUnit.Framework;

namespace TestingLab.Tests.EditMode.Exercises
{
    // Practice: these classes already work. Fill in tests for the behaviours below.
    // Remove the [Ignore] attribute when you start a test.
    public class MoreExercises
    {
        [Test, Ignore("Exercise: Inventory.Remove with more than you have returns false and changes nothing")]
        public void Inventory_RemoveMoreThanHeld_ReturnsFalse()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise: Heal raises Changed, but NOT when already at full health")]
        public void Health_HealAtFullHealth_DoesNotRaiseChanged()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise: ScoreKeeper.AddPoints with a negative number throws")]
        public void ScoreKeeper_NegativePoints_Throws()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise: a crit with armor 100 on 100 base damage deals 100")]
        public void DamageCalculator_CritWithArmor()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise: DamageCalculator rejects a crit chance above 1 (use [TestCase] for several bad values)")]
        public void DamageCalculator_InvalidCritChance_Throws()
        {
            Assert.Fail("TODO");
        }
    }
}
