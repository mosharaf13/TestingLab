using System;
using System.Collections.Generic;
using NUnit.Framework;
using TestingLab.Core.Session;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Level2_Intermediate
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Code under test: Assets/Scripts/Core/Session/GameSession.cs
    // In the game:     timer, coin goal, pause, win and lose in the Arena.
    //
    // Topics: testing a STATE MACHINE (every legal and illegal transition),
    //         [TestCaseSource] for data that [TestCase] can't express,
    //         recording a sequence of events.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Intermediate)]
    public class GameSessionTests
    {
        private GameSession _session;

        [SetUp]
        public void SetUp()
        {
            _session = new GameSession(timeLimitSeconds: 30f, coinGoal: 5);
        }

        [Test]
        public void NewSession_IsReady_WithFullTime()
        {
            Assert.That(_session.Phase, Is.EqualTo(GamePhase.Ready));
            Assert.That(_session.TimeRemaining, Is.EqualTo(30f));
        }

        [Test]
        public void ReachingCoinGoal_WinsTheLevel()
        {
            _session.Start();

            _session.CollectCoins(4);
            Assert.That(_session.Phase, Is.EqualTo(GamePhase.Playing), "4 of 5 coins is not enough");

            _session.CollectCoins(1);
            Assert.That(_session.Phase, Is.EqualTo(GamePhase.Won));
        }

        [Test]
        public void RunningOutOfTime_LosesTheLevel()
        {
            _session.Start();

            // Careful with floats! 30 - 29.9f - 0.1f is NOT exactly 0 (it's 0.0000004),
            // so a test using those numbers fails for a reason that has nothing to do
            // with the game. Use values that are exact in binary (0.5, 0.25...).
            _session.Tick(29.5f);
            Assert.That(_session.Phase, Is.EqualTo(GamePhase.Playing));

            _session.Tick(0.5f);
            Assert.That(_session.Phase, Is.EqualTo(GamePhase.Lost));
            Assert.That(_session.LoseReason, Is.EqualTo(LoseReason.TimeUp));
            Assert.That(_session.TimeRemaining, Is.EqualTo(0f), "time never goes negative");
        }

        [Test]
        public void PlayerDying_LosesTheLevel()
        {
            _session.Start();

            _session.PlayerDied();

            Assert.That(_session.Phase, Is.EqualTo(GamePhase.Lost));
            Assert.That(_session.LoseReason, Is.EqualTo(LoseReason.PlayerDied));
        }

        [Test]
        public void WhilePaused_TimeAndCoinsDoNotCount()
        {
            _session.Start();
            _session.Pause();

            _session.Tick(100f);
            _session.CollectCoins(5);

            Assert.That(_session.Phase, Is.EqualTo(GamePhase.Paused));
            Assert.That(_session.TimeRemaining, Is.EqualTo(30f));
            Assert.That(_session.CoinsCollected, Is.EqualTo(0));
        }

        [Test]
        public void TogglePause_SwitchesBetweenPlayingAndPaused()
        {
            _session.Start();

            _session.TogglePause();
            Assert.That(_session.Phase, Is.EqualTo(GamePhase.Paused));

            _session.TogglePause();
            Assert.That(_session.Phase, Is.EqualTo(GamePhase.Playing));
        }

        [Test]
        public void PhaseChanged_ReportsEveryTransitionInOrder()
        {
            var transitions = new List<string>();
            _session.PhaseChanged += (from, to) => transitions.Add($"{from}->{to}");

            _session.Start();
            _session.Pause();
            _session.Resume();
            _session.CollectCoins(5);

            Assert.That(transitions, Is.EqualTo(new[]
            {
                "Ready->Playing", "Playing->Paused", "Paused->Playing", "Playing->Won"
            }));
        }

        // ── Illegal transitions ────────────────────────────────────────────────
        // [TestCaseSource] points at a method that yields test data. Each item
        // becomes its own test in the Test Runner, with the name we give it.

        private static IEnumerable<TestCaseData> IllegalMoves()
        {
            yield return new TestCaseData((Action<GameSession>)(s => s.Start()), (Action<GameSession>)(s => s.Start()))
                .SetName("Start twice");
            yield return new TestCaseData((Action<GameSession>)(s => { }), (Action<GameSession>)(s => s.Pause()))
                .SetName("Pause before Start");
            yield return new TestCaseData((Action<GameSession>)(s => s.Start()), (Action<GameSession>)(s => s.Resume()))
                .SetName("Resume while Playing");
            yield return new TestCaseData((Action<GameSession>)(s => { s.Start(); s.PlayerDied(); }), (Action<GameSession>)(s => s.Pause()))
                .SetName("Pause after losing");
        }

        [TestCaseSource(nameof(IllegalMoves))]
        public void IllegalTransition_Throws(Action<GameSession> setup, Action<GameSession> illegalMove)
        {
            setup(_session);

            Assert.Throws<InvalidOperationException>(() => illegalMove(_session));
        }

        [TestCase(0f, 5)]
        [TestCase(30f, 0)]
        [TestCase(-1f, 5)]
        public void Constructor_RejectsNonsenseSettings(float timeLimit, int goal)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new GameSession(timeLimit, goal));
        }
    }
}
