using NUnit.Framework;

namespace TestingLab.Tests.EditMode
{
    // LESSON 4: test doubles. FakeTimeProvider lets us "wait" 2 seconds instantly.
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
        public void FirstHit_HasMultiplierOfOne()
        {
            _score.AddPoints(10);

            Assert.That(_score.Multiplier, Is.EqualTo(1));
            Assert.That(_score.Score, Is.EqualTo(10));
        }

        [Test]
        public void QuickSecondHit_DoublesPoints()
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
        public void SlowHit_ResetsCombo()
        {
            _score.AddPoints(10);
            _clock.Advance(0.5f);
            _score.AddPoints(10);   // x2
            _clock.Advance(5f);     // too slow
            _score.AddPoints(10);   // back to x1

            Assert.That(_score.Multiplier, Is.EqualTo(1));
        }

        [Test]
        public void HitExactlyAtWindowEdge_StillCountsAsCombo()
        {
            // Boundary test: the window is inclusive (<=). Off-by-one bugs live here.
            _score.AddPoints(10);
            _clock.Advance(2f);
            _score.AddPoints(10);

            Assert.That(_score.Multiplier, Is.EqualTo(2));
        }
    }
}
