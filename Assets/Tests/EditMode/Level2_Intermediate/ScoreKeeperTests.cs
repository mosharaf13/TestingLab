using System;
using NUnit.Framework;
using TestingLab.Core.Scoring;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Fakes;

namespace TestingLab.Tests.EditMode.Level2_Intermediate
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Code under test: Assets/Scripts/Core/Scoring/ScoreKeeper.cs
    // In the game:     "Score 120  x3" on the Arena HUD.
    //
    // Topic: TEST DOUBLES — controlling time.
    // The combo depends on how many seconds pass between pickups. A test that
    // really waited would be slow and flaky. FakeTimeProvider lets us say
    // "now 2 seconds have passed" instantly and exactly.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Intermediate)]
    public class ScoreKeeperTests
    {
        private FakeTimeProvider _clock;
        private ScoreKeeper _score;

        [SetUp]
        public void SetUp()
        {
            _clock = new FakeTimeProvider();
            _score = new ScoreKeeper(_clock, comboWindowSeconds: 2f);
        }

        [Test]
        public void FirstPickup_HasMultiplierOfOne()
        {
            _score.AddPoints(10);

            Assert.That(_score.Multiplier, Is.EqualTo(1));
            Assert.That(_score.Score, Is.EqualTo(10));
        }

        [Test]
        public void QuickSecondPickup_DoublesPoints()
        {
            _score.AddPoints(10);
            _clock.Advance(1f);
            _score.AddPoints(10);

            Assert.That(_score.Multiplier, Is.EqualTo(2));
            Assert.That(_score.Score, Is.EqualTo(10 + 20));
        }

        [Test]
        public void Multiplier_IsCappedAtMax()
        {
            for (int i = 0; i < 10; i++)
            {
                _score.AddPoints(1);
                _clock.Advance(0.5f);
            }

            Assert.That(_score.Multiplier, Is.EqualTo(ScoreKeeper.MaxMultiplier));
        }

        [Test]
        public void SlowPickup_ResetsCombo()
        {
            _score.AddPoints(10);
            _clock.Advance(0.5f);
            _score.AddPoints(10);   // x2
            _clock.Advance(5f);     // too slow
            _score.AddPoints(10);   // back to x1

            Assert.That(_score.Multiplier, Is.EqualTo(1));
        }

        [Test]
        public void PickupExactlyAtWindowEdge_StillCountsAsCombo()
        {
            // Boundary test: the window is inclusive (<=). Off-by-one bugs live here.
            // Try changing <= to < in ScoreKeeper and see which test catches it.
            _score.AddPoints(10);
            _clock.Advance(2f);
            _score.AddPoints(10);

            Assert.That(_score.Multiplier, Is.EqualTo(2));
        }

        [Test]
        public void PointsAdded_ReportsAwardedPointsAndMultiplier()
        {
            int awarded = 0, multiplier = 0;
            _score.PointsAdded += (points, mult) => { awarded = points; multiplier = mult; };

            _score.AddPoints(10);
            _clock.Advance(1f);
            _score.AddPoints(10);

            Assert.That(awarded, Is.EqualTo(20));
            Assert.That(multiplier, Is.EqualTo(2));
        }

        [Test]
        public void Constructor_NullClock_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new ScoreKeeper(null));
        }
    }
}
