using System;

namespace TestingLab.Core.Session
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE  (state machines, table-driven tests)
    // Tested by:      Tests/EditMode/Level2_Intermediate/GameSessionTests.cs
    // Pattern:        Finite State Machine with guarded transitions
    //
    // The RULES of one level of the Arena, with no Unity code at all:
    //   Ready ──Start──▶ Playing ◀──Resume── Paused
    //                     │  └──────Pause──────▲
    //                     ├─ coins >= goal ──▶ Won
    //                     └─ time up / died ─▶ Lost
    //
    // ArenaController (a MonoBehaviour) just forwards Update's deltaTime and
    // gameplay events into this class, then shows whatever phase it reports.
    // Illegal moves (e.g. Pause while Won) throw, so bugs are loud, not silent.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class GameSession
    {
        public GamePhase Phase { get; private set; } = GamePhase.Ready;
        public LoseReason LoseReason { get; private set; } = LoseReason.None;
        public float TimeLimitSeconds { get; }
        public float TimeRemaining { get; private set; }
        public int CoinGoal { get; }
        public int CoinsCollected { get; private set; }
        public bool IsOver => Phase == GamePhase.Won || Phase == GamePhase.Lost;

        /// <summary>Raised on every transition: (from, to).</summary>
        public event Action<GamePhase, GamePhase> PhaseChanged;

        public GameSession(float timeLimitSeconds, int coinGoal)
        {
            if (timeLimitSeconds <= 0f)
                throw new ArgumentOutOfRangeException(nameof(timeLimitSeconds), "Time limit must be positive.");
            if (coinGoal <= 0)
                throw new ArgumentOutOfRangeException(nameof(coinGoal), "Coin goal must be positive.");

            TimeLimitSeconds = timeLimitSeconds;
            TimeRemaining = timeLimitSeconds;
            CoinGoal = coinGoal;
        }

        public void Start() => TransitionFrom(GamePhase.Ready, GamePhase.Playing);

        public void Pause() => TransitionFrom(GamePhase.Playing, GamePhase.Paused);

        public void Resume() => TransitionFrom(GamePhase.Paused, GamePhase.Playing);

        /// <summary>Esc key behaviour: pause if playing, resume if paused, otherwise ignore.</summary>
        public void TogglePause()
        {
            if (Phase == GamePhase.Playing)
                Pause();
            else if (Phase == GamePhase.Paused)
                Resume();
        }

        /// <summary>Advance the clock. Does nothing unless Playing.</summary>
        public void Tick(float deltaSeconds)
        {
            if (deltaSeconds < 0f)
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            if (Phase != GamePhase.Playing)
                return;

            TimeRemaining = Math.Max(0f, TimeRemaining - deltaSeconds);
            if (TimeRemaining <= 0f)
                Lose(LoseReason.TimeUp);
        }

        /// <summary>Count collected coins. Ignored unless Playing.</summary>
        public void CollectCoins(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (Phase != GamePhase.Playing)
                return;

            CoinsCollected += amount;
            if (CoinsCollected >= CoinGoal)
                ChangePhase(GamePhase.Won);
        }

        public void PlayerDied()
        {
            if (Phase == GamePhase.Playing || Phase == GamePhase.Paused)
                Lose(LoseReason.PlayerDied);
        }

        private void Lose(LoseReason reason)
        {
            LoseReason = reason;
            ChangePhase(GamePhase.Lost);
        }

        private void TransitionFrom(GamePhase expected, GamePhase next)
        {
            if (Phase != expected)
                throw new InvalidOperationException($"Cannot go to {next} from {Phase} (expected {expected}).");

            ChangePhase(next);
        }

        private void ChangePhase(GamePhase next)
        {
            var previous = Phase;
            Phase = next;
            PhaseChanged?.Invoke(previous, next);
        }
    }
}
