using NUnit.Framework;
using TestingLab.Core.Common;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Fakes;

namespace TestingLab.Tests.EditMode.Level2_Intermediate
{
    // Learning level: INTERMEDIATE
    // Code under test: Assets/Scripts/Core/Common/Cooldown.cs
    // In the game: after an enemy hits you, you can't be hit again for 0.75 s.

    [Category(TestCategory.Intermediate)]
    public class CooldownTests
    {
        private FakeTimeProvider _clock;
        private Cooldown _cooldown;

        [SetUp]
        public void SetUp()
        {
            _clock = new FakeTimeProvider();
            _cooldown = new Cooldown(_clock, durationSeconds: 1f);
        }

        [Test]
        public void NewCooldown_IsReady()
        {
            Assert.That(_cooldown.IsReady, Is.True);
        }

        [Test]
        public void TryUse_Twice_SecondUseIsBlocked()
        {
            Assert.That(_cooldown.TryUse(), Is.True);
            Assert.That(_cooldown.TryUse(), Is.False);
        }

        [Test]
        public void TryUse_AfterDuration_IsAllowedAgain()
        {
            _cooldown.TryUse();
            _clock.Advance(1f); // exactly the duration: boundary!

            Assert.That(_cooldown.TryUse(), Is.True);
        }

        [Test]
        public void RemainingSeconds_CountsDown()
        {
            _cooldown.TryUse();
            _clock.Advance(0.25f);

            Assert.That(_cooldown.RemainingSeconds, Is.EqualTo(0.75f).Within(0.0001f));
        }
    }
}
