using System;
using NUnit.Framework;

namespace TestingLab.Tests.EditMode
{
    // LESSON 1-2: first tests, Arrange-Act-Assert, exceptions, events.
    public class HealthTests
    {
        [Test]
        public void NewHealth_StartsAtMax()
        {
            var health = new Health(100);

            Assert.That(health.Current, Is.EqualTo(100));
            Assert.That(health.IsDead, Is.False);
        }

        [Test]
        public void TakeDamage_ReducesCurrentHealth()
        {
            // Arrange
            var health = new Health(100);

            // Act
            health.TakeDamage(30);

            // Assert
            Assert.That(health.Current, Is.EqualTo(70));
        }

        [Test]
        public void TakeDamage_MoreThanCurrent_ClampsToZero()
        {
            var health = new Health(50);

            health.TakeDamage(999);

            Assert.That(health.Current, Is.EqualTo(0));
            Assert.That(health.IsDead, Is.True);
        }

        [Test]
        public void TakeDamage_Negative_Throws()
        {
            var health = new Health(100);

            Assert.Throws<ArgumentOutOfRangeException>(() => health.TakeDamage(-5));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-100)]
        public void Constructor_NonPositiveMax_Throws(int max)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Health(max));
        }

        [Test]
        public void Died_IsRaisedExactlyOnce()
        {
            var health = new Health(100);
            int diedCount = 0;
            health.Died += () => diedCount++;

            health.TakeDamage(100);
            health.TakeDamage(10); // already dead: should not fire again

            Assert.That(diedCount, Is.EqualTo(1));
        }

        [Test]
        public void Changed_ReportsOldAndNewValues()
        {
            var health = new Health(100);
            int reportedOld = -1, reportedNew = -1;
            health.Changed += (oldValue, newValue) =>
            {
                reportedOld = oldValue;
                reportedNew = newValue;
            };

            health.TakeDamage(25);

            Assert.That(reportedOld, Is.EqualTo(100));
            Assert.That(reportedNew, Is.EqualTo(75));
        }

        [Test]
        public void Heal_CannotExceedMax()
        {
            var health = new Health(100);
            health.TakeDamage(10);

            health.Heal(50);

            Assert.That(health.Current, Is.EqualTo(100));
        }

        [Test]
        public void Heal_WhenDead_DoesNothing()
        {
            var health = new Health(100);
            health.TakeDamage(100);

            health.Heal(50);

            Assert.That(health.Current, Is.EqualTo(0));
        }

        [Test]
        public void Percent_IsFractionOfMax()
        {
            var health = new Health(200);
            health.TakeDamage(150);

            // Floats: compare with a tolerance, never with exact equality.
            Assert.That(health.Percent, Is.EqualTo(0.25f).Within(0.0001f));
        }
    }
}
