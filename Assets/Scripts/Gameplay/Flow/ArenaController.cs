using TestingLab.Core.Presentation;
using TestingLab.Core.Progression;
using TestingLab.Core.Scoring;
using TestingLab.Core.Session;
using TestingLab.Core.Waves;
using TestingLab.Gameplay.Combat;
using TestingLab.Gameplay.Config;
using TestingLab.Gameplay.Infrastructure;
using TestingLab.Gameplay.Pickups;
using TestingLab.Gameplay.Player;
using TestingLab.Gameplay.Spawning;
using TestingLab.Gameplay.UI;
using UnityEngine;

namespace TestingLab.Gameplay.Flow
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED  (integration / scene tests)
    // Tested by:      Tests/PlayMode/Level3_Advanced/ArenaSceneTests.cs
    // Pattern:        Scene composition root + Mediator
    //
    // The "conductor" of the Arena scene. It creates the plain C# systems
    // (GameSession, ScoreKeeper, WaveSchedule), connects their events to the
    // MonoBehaviours and views, and forwards Update's deltaTime.
    //
    // Notice how LITTLE logic is here: every rule lives in a class that has its own
    // fast Edit Mode tests. What's left — "are things wired together correctly?" —
    // is exactly what the slower Play Mode integration tests check.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class ArenaController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private LevelCatalog levels;

        [Header("Scene objects")]
        [SerializeField] private PlayerController player;
        [SerializeField] private WaveSpawner spawner;

        [Header("UI")]
        [SerializeField] private HudView hud;
        [SerializeField] private PauseView pauseView;
        [SerializeField] private ResultsView resultsView;

        private HealthComponent _playerHealth;
        private DamageReceiver _playerDamage;
        private Collector _collector;

        public GameSession Session { get; private set; }
        public ScoreKeeper Score { get; private set; }
        public RunProgression Progression { get; private set; }
        public LevelConfig CurrentLevel { get; private set; }
        public PlayerController Player => player;
        public WaveSpawner Spawner => spawner;

        // Start (not Awake) so every other component's Awake has already run,
        // e.g. HealthComponent has created its Health object.
        private void Start()
        {
            _playerHealth = player.GetComponent<HealthComponent>();
            _playerDamage = player.GetComponent<DamageReceiver>();
            _collector = player.GetComponent<Collector>();

            Progression = GameServices.CreateProgression(levels.Count);
            int levelIndex = Mathf.Min(Progression.Profile.LevelIndex, levels.Count - 1);
            CurrentLevel = levels.Get(levelIndex);

            ApplyStats(Progression.Profile.Stats);

            Session = new GameSession(CurrentLevel.TimeLimitSeconds, CurrentLevel.CoinGoal);
            Score = new ScoreKeeper(new UnityTimeProvider());

            var picker = new SpawnPointPicker(new UnityRandomSource(), player.ArenaHalfExtents);
            spawner.Initialise(new WaveSchedule(CurrentLevel.BuildWaves()), picker, player.transform, CurrentLevel);

            WireEvents();

            hud.SetLevel(HudFormatter.Level(levelIndex, levels.Count, CurrentLevel.DisplayName));
            pauseView.Hide();
            resultsView.Hide();

            Time.timeScale = 1f;
            Session.Start();
            spawner.Tick(0f); // release the waves that start at t = 0
            RefreshHud();
        }

        private void Update()
        {
            if (Session == null)
                return;

            if (PausePressedThisFrame())
                Session.TogglePause();

            float dt = Time.deltaTime;
            Session.Tick(dt);
            if (Session.Phase == GamePhase.Playing)
                spawner.Tick(dt);

            RefreshHud();
        }

        private void OnDestroy()
        {
            // Leaving the scene (or a test tearing it down) must never leave the game frozen.
            Time.timeScale = 1f;
        }

        private void ApplyStats(PlayerStats stats)
        {
            _playerHealth.SetDestroyOnDeath(false);
            _playerHealth.Initialise(stats.MaxHealth);
            _playerDamage.Armor = stats.Armor;
            player.MoveSpeed = stats.MoveSpeed;
        }

        private void WireEvents()
        {
            _collector.Collected += OnCollected;
            _playerHealth.Died += Session.PlayerDied;
            _playerDamage.Damaged += hud.ShowDamage;
            Session.PhaseChanged += OnPhaseChanged;

            hud.PauseClicked += Session.TogglePause;
            pauseView.ResumeClicked += Session.Resume;
            pauseView.QuitClicked += Progression.QuitToMenu;
            resultsView.ContinueClicked += Progression.ContinueAfterWin;
            resultsView.RetryClicked += Progression.RetryLevel;
            resultsView.MenuClicked += Progression.QuitToMenu;
        }

        private void OnCollected(string itemId, int amount)
        {
            if (itemId != Collectible.CoinId)
                return;

            // Score first, so the winning coin's points are in the final score.
            Score.AddPoints(CurrentLevel.PointsPerCoin * amount);
            Session.CollectCoins(amount);
        }

        private void OnPhaseChanged(GamePhase from, GamePhase to)
        {
            switch (to)
            {
                case GamePhase.Paused:
                    Time.timeScale = 0f;
                    pauseView.Show();
                    break;
                case GamePhase.Playing when from == GamePhase.Paused:
                    Time.timeScale = 1f;
                    pauseView.Hide();
                    break;
                case GamePhase.Won:
                case GamePhase.Lost:
                    EndLevel(to == GamePhase.Won);
                    break;
            }
        }

        private void EndLevel(bool won)
        {
            player.enabled = false;
            pauseView.Hide();
            Time.timeScale = 0f; // freeze enemies; UI still works on unscaled time

            Progression.RecordLevelResult(won, Session.CoinsCollected, Score.Score);

            string title = ResultsFormatter.Title(won, Session.LoseReason);
            string summary = ResultsFormatter.Summary(Score.Score, Session.CoinsCollected, Progression.Profile.BestScore);
            if (won)
                resultsView.ShowWin(title, summary, ResultsFormatter.ContinueLabel(Progression.IsRunComplete));
            else
                resultsView.ShowLoss(title, summary);
        }

        private void RefreshHud()
        {
            hud.SetScore(Score.Score, Score.Multiplier);
            hud.SetCoins(Session.CoinsCollected, Session.CoinGoal);
            hud.SetTimeRemaining(Session.TimeRemaining);
            if (_playerHealth.Health != null)
                hud.SetHealth(_playerHealth.Health.Current, _playerHealth.Health.Max);
        }

        private static bool PausePressedThisFrame()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P);
#else
            return false;
#endif
        }
    }
}
