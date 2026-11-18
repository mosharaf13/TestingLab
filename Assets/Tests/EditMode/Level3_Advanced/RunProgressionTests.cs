using System;
using NUnit.Framework;
using TestingLab.Core.Progression;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Builders;
using TestingLab.Tests.Shared.Fakes;

namespace TestingLab.Tests.EditMode.Level3_Advanced
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED
    // Code under test: Assets/Scripts/Core/Progression/RunProgression.cs
    // In the game:     Menu → Arena → Shop → Arena … the whole run.
    //
    // Topic: INTERACTION TESTING with spies.
    //   State-based test:        "after X, the value is Y"       (most tests so far)
    //   Interaction-based test:  "after X, it TOLD something to do Y"
    //
    // RunProgression's job is to tell the navigator where to go and the store to
    // save. We can't (and don't want to) load real scenes here, so we hand it a
    // SpyGameNavigator and a FakeProfileStore and check what they were asked.
    //
    // Use interaction tests sparingly: they know HOW the code works, so they break
    // more often when you refactor. Prefer state checks when there is state.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Advanced)]
    public class RunProgressionTests
    {
        private const int LevelCount = 3;

        private FakeProfileStore _store;
        private SpyGameNavigator _navigator;

        [SetUp]
        public void SetUp()
        {
            _store = new FakeProfileStore();
            _navigator = new SpyGameNavigator();
        }

        private RunProgression CreateProgression() => new RunProgression(_store, _navigator, LevelCount);

        [Test]
        public void NoSave_StartsWithAFreshProfile()
        {
            var progression = CreateProgression();

            Assert.That(progression.Profile.LevelIndex, Is.EqualTo(0));
            Assert.That(progression.HasRunInProgress, Is.False);
        }

        [Test]
        public void StartNewRun_SavesAFreshProfile_KeepsBestScore_AndGoesToArena()
        {
            _store.Stored = A.Profile().AtLevel(2).WithCoins(40).WithBestScore(777);
            var progression = CreateProgression();

            progression.StartNewRun();

            Assert.That(_store.SaveCount, Is.EqualTo(1));
            Assert.That(_store.Stored.LevelIndex, Is.EqualTo(0));
            Assert.That(_store.Stored.Wallet.Coins, Is.EqualTo(0));
            Assert.That(_store.Stored.BestScore, Is.EqualTo(777));
            Assert.That(_navigator.Calls, Is.EqualTo(new[] { SpyGameNavigator.Arena }));
        }

        [Test]
        public void WinningAMiddleLevel_ContinuesToTheShop()
        {
            var progression = CreateProgression();

            progression.RecordLevelResult(won: true, coinsCollected: 10, levelScore: 100);
            progression.ContinueAfterWin();

            Assert.That(_navigator.LastDestination, Is.EqualTo(SpyGameNavigator.Shop));
        }

        [Test]
        public void WinningTheLastLevel_ReturnsToMainMenu()
        {
            _store.Stored = A.Profile().AtLevel(LevelCount - 1);
            var progression = CreateProgression();

            progression.RecordLevelResult(won: true, coinsCollected: 10, levelScore: 100);
            progression.ContinueAfterWin();

            Assert.That(progression.IsRunComplete, Is.True);
            Assert.That(_navigator.LastDestination, Is.EqualTo(SpyGameNavigator.MainMenu));
        }

        [Test]
        public void RecordLevelResult_AlwaysSaves()
        {
            var progression = CreateProgression();

            progression.RecordLevelResult(won: false, coinsCollected: 0, levelScore: 0);

            Assert.That(_store.SaveCount, Is.EqualTo(1));
        }

        [Test]
        public void LeaveShop_SavesBeforeLoadingTheArena()
        {
            int savesWhenNavigating = -1;
            // A tiny inline spy: check ORDER by recording the save count at the moment of navigation.
            var orderSpy = new OrderCheckingNavigator(() => savesWhenNavigating = _store.SaveCount);
            var progression = new RunProgression(_store, orderSpy, LevelCount);

            progression.LeaveShop();

            Assert.That(savesWhenNavigating, Is.EqualTo(1), "the profile must be saved BEFORE the scene changes");
        }

        [Test]
        public void ContinueRun_WithoutARunInProgress_Throws()
        {
            var progression = CreateProgression();

            Assert.Throws<InvalidOperationException>(progression.ContinueRun);
            Assert.That(_navigator.Calls, Is.Empty, "a failed call must not navigate anywhere");
        }

        private sealed class OrderCheckingNavigator : IGameNavigator
        {
            private readonly Action _onNavigate;
            public OrderCheckingNavigator(Action onNavigate) => _onNavigate = onNavigate;
            public void GoToMainMenu() => _onNavigate();
            public void GoToArena() => _onNavigate();
            public void GoToShop() => _onNavigate();
        }
    }
}
