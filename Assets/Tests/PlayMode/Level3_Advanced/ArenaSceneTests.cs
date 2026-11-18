using System.Collections;
using System.Linq;
using NUnit.Framework;
using TestingLab.Core.Session;
using TestingLab.Gameplay.Combat;
using TestingLab.Gameplay.Flow;
using TestingLab.Gameplay.Infrastructure;
using TestingLab.Gameplay.Pickups;
using TestingLab.Gameplay.UI;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Support;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace TestingLab.Tests.PlayMode.Level3_Advanced
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED
    // Scene under test: Assets/Scenes/Arena.unity
    //
    // Topic: END-TO-END gameplay tests. The real scene, real physics, real UI.
    // The test "plays" by teleporting the player onto coins — a simple, reliable
    // bot. GodMode stops enemies from making the test random.
    //
    // Question for class: which bugs could ONLY these tests catch? (Hint: a
    // missing event subscription in ArenaController, a wrong prefab reference,
    // the HUD not updating, Time.timeScale left at 0...)
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Advanced), Category(TestCategory.Integration)]
    [Timeout(60000)] // safety net: a stuck scene test fails after 60 s instead of hanging the runner
    public class ArenaSceneTests : SceneTestBase
    {
        private ArenaController _arena;

        private IEnumerator LoadArena()
        {
            yield return SceneTestHelpers.LoadScene(SceneNames.Arena);
            _arena = Object.FindFirstObjectByType<ArenaController>();
            Assert.That(_arena, Is.Not.Null, "Arena scene has no ArenaController");
            yield return WaitFor.Condition(() => _arena.Session != null, 2f, "ArenaController never started");
            _arena.Player.GetComponent<DamageReceiver>().GodMode = true;
        }

        [UnityTest]
        public IEnumerator LevelStarts_PlayingLevelOne_WithCoinsOnTheField()
        {
            yield return LoadArena();

            Assert.That(_arena.Session.Phase, Is.EqualTo(GamePhase.Playing));
            Assert.That(_arena.CurrentLevel.DisplayName, Is.EqualTo("Warm-up"));
            Assert.That(_arena.Spawner.CoinsSpawned, Is.GreaterThan(0), "the t=0 wave should spawn immediately");
            Assert.That(Object.FindFirstObjectByType<HudView>().CoinsText, Is.EqualTo($"Coins 0/{_arena.Session.CoinGoal}"));
        }

        [UnityTest]
        public IEnumerator TouchingACoin_UpdatesScoreSessionAndHud()
        {
            yield return LoadArena();
            var hud = Object.FindFirstObjectByType<HudView>();

            yield return CollectOneCoin();

            Assert.That(_arena.Session.CoinsCollected, Is.EqualTo(1));
            Assert.That(_arena.Score.Score, Is.EqualTo(_arena.CurrentLevel.PointsPerCoin));
            Assert.That(hud.ScoreText, Is.EqualTo($"Score {_arena.CurrentLevel.PointsPerCoin}"));
        }

        [UnityTest]
        public IEnumerator CollectingEnoughCoins_WinsTheLevel_AndSavesProgress()
        {
            yield return LoadArena();
            Time.timeScale = 5f; // fast-forward so later waves arrive sooner

            float giveUpAt = Time.realtimeSinceStartup + 30f;
            while (_arena.Session.Phase == GamePhase.Playing && Time.realtimeSinceStartup < giveUpAt)
                yield return CollectOneCoin();

            Assert.That(_arena.Session.Phase, Is.EqualTo(GamePhase.Won));
            var results = Object.FindFirstObjectByType<ResultsView>();
            Assert.That(results.IsVisible, Is.True);
            Assert.That(results.Title, Is.EqualTo("Level Complete!"));
            Assert.That(SaveData.Load().LevelIndex, Is.EqualTo(1), "winning should be saved");
            Assert.That(Time.timeScale, Is.EqualTo(0f), "the game freezes behind the results panel");
        }

        [UnityTest]
        public IEnumerator PlayerDeath_ShowsGameOver_AndRetryReloadsTheArena()
        {
            yield return LoadArena();
            var playerHealth = _arena.Player.GetComponent<HealthComponent>();

            playerHealth.Health.TakeDamage(9999);
            yield return null;

            var results = Object.FindFirstObjectByType<ResultsView>();
            Assert.That(results.IsVisible, Is.True);
            Assert.That(results.Title, Is.EqualTo("Game Over"));
            Assert.That(UiTestHelpers.IsVisible("ContinueButton"), Is.False, "you can't continue after losing");

            var oldArena = _arena;
            UiTestHelpers.Click("RetryButton");
            yield return WaitFor.Condition(() =>
            {
                var current = Object.FindFirstObjectByType<ArenaController>();
                return current != null && current != oldArena && current.Session != null;
            }, 5f, "Arena never reloaded");

            Assert.That(Time.timeScale, Is.EqualTo(1f), "a retry must unfreeze the game");
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo(SceneNames.Arena));
        }

        [UnityTest]
        public IEnumerator PauseButton_FreezesTheGame_AndResumeUnfreezesIt()
        {
            yield return LoadArena();
            var pause = Object.FindFirstObjectByType<PauseView>();

            UiTestHelpers.Click("PauseButton");
            Assert.That(_arena.Session.Phase, Is.EqualTo(GamePhase.Paused));
            Assert.That(Time.timeScale, Is.EqualTo(0f));
            Assert.That(pause.IsVisible, Is.True);

            float timeLeft = _arena.Session.TimeRemaining;
            yield return WaitFor.Frames(10);
            Assert.That(_arena.Session.TimeRemaining, Is.EqualTo(timeLeft), "the timer must not run while paused");

            UiTestHelpers.Click("ResumeButton");
            Assert.That(_arena.Session.Phase, Is.EqualTo(GamePhase.Playing));
            Assert.That(Time.timeScale, Is.EqualTo(1f));
            Assert.That(pause.IsVisible, Is.False);
        }

        /// <summary>The "bot": teleport onto the nearest coin and let physics do the rest.</summary>
        private IEnumerator CollectOneCoin()
        {
            yield return WaitFor.Condition(() => Object.FindObjectsByType<Collectible>(FindObjectsSortMode.None).Length > 0,
                10f, "no coins ever appeared");

            var player = _arena.Player.transform;
            var coin = Object.FindObjectsByType<Collectible>(FindObjectsSortMode.None)
                .OrderBy(c => Vector3.Distance(c.transform.position, player.position))
                .First();

            player.position = new Vector3(coin.transform.position.x, player.position.y, coin.transform.position.z);
            yield return WaitFor.Physics();
        }
    }
}
