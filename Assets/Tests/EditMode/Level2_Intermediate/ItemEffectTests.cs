using System;
using NUnit.Framework;
using TestingLab.Core.Progression;
using TestingLab.Core.Shop;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Level2_Intermediate
{
    // Learning level: INTERMEDIATE
    // Code under test: Assets/Scripts/Core/Shop/*Boost.cs (Strategy pattern)
    // Each strategy is tiny, so each test is tiny. That's the point of the pattern.

    [Category(TestCategory.Intermediate)]
    public class ItemEffectTests
    {
        private PlayerStats _stats;

        [SetUp]
        public void SetUp() => _stats = new PlayerStats(maxHealth: 100, armor: 0, moveSpeed: 6f);

        [Test]
        public void MaxHealthBoost_RaisesMaxHealth()
        {
            new MaxHealthBoost(25).Apply(_stats);

            Assert.That(_stats.MaxHealth, Is.EqualTo(125));
        }

        [Test]
        public void ArmorBoost_RaisesArmor()
        {
            new ArmorBoost(20).Apply(_stats);

            Assert.That(_stats.Armor, Is.EqualTo(20));
        }

        [Test]
        public void SpeedBoost_RaisesMoveSpeed()
        {
            new SpeedBoost(1.5f).Apply(_stats);

            Assert.That(_stats.MoveSpeed, Is.EqualTo(7.5f).Within(0.0001f));
        }

        [Test]
        public void Effects_StackWhenAppliedTwice()
        {
            var boost = new ArmorBoost(10);

            boost.Apply(_stats);
            boost.Apply(_stats);

            Assert.That(_stats.Armor, Is.EqualTo(20));
        }

        [Test]
        public void Descriptions_AreReadable()
        {
            Assert.That(new MaxHealthBoost(25).Description, Is.EqualTo("+25 max health"));
            Assert.That(new SpeedBoost(1f).Description, Is.EqualTo("+1 move speed"));
        }

        [Test]
        public void ZeroAmount_IsRejected()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ArmorBoost(0));
        }
    }
}
