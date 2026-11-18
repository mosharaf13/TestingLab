using NUnit.Framework;
using TestingLab.Core.Combat;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Fakes;

namespace TestingLab.Tests.EditMode.Level2_Intermediate
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Code under test: Assets/Scripts/Core/Combat/DamageCalculator.cs
    // In the game:     how much an enemy's touch hurts, after your Armor upgrades.
    //
    // Topic: TEST DOUBLES — controlling randomness.
    // A 10% crit chance means a real-random test would fail 1 run in 10.
    // FakeRandomSource lets each test choose the "roll".
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Intermediate)]
    public class DamageCalculatorTests
    {
        private FakeRandomSource _random;
        private DamageCalculator _calculator;

        [SetUp]
        public void SetUp()
        {
            _random = new FakeRandomSource { NextValue = 0.99f }; // no crit by default
            _calculator = new DamageCalculator(_random, critChance: 0.1f, critMultiplier: 2f);
        }

        [TestCase(100, 0, ExpectedResult = 100)]
        [TestCase(100, 100, ExpectedResult = 50)]
        [TestCase(100, 50, ExpectedResult = 67)]   // 66.67 rounds up
        [TestCase(1, 1000, ExpectedResult = 1)]    // minimum damage is 1
        [TestCase(0, 0, ExpectedResult = 0)]       // ...unless base damage is 0
        public int Calculate_AppliesArmor(int baseDamage, int armor)
        {
            return _calculator.Calculate(baseDamage, armor);
        }

        [Test]
        public void Calculate_LowRoll_IsCriticalHit()
        {
            _random.NextValue = 0.05f;

            int damage = _calculator.Calculate(100, 0);

            Assert.That(damage, Is.EqualTo(200));
            Assert.That(_calculator.LastHitWasCrit, Is.True);
        }

        [Test]
        public void Calculate_RollEqualToCritChance_IsNotCritical()
        {
            _random.NextValue = 0.1f;

            _calculator.Calculate(100, 0);

            Assert.That(_calculator.LastHitWasCrit, Is.False);
        }

        [Test]
        public void Calculate_RollsTheDiceExactlyOncePerHit()
        {
            _calculator.Calculate(10, 0);
            _calculator.Calculate(10, 0);

            Assert.That(_random.CallCount, Is.EqualTo(2));
        }
    }
}
