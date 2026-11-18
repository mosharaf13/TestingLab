using System;
using TestingLab.Core.Waves;
using TestingLab.Gameplay.Combat;
using TestingLab.Gameplay.Config;
using TestingLab.Gameplay.Enemies;
using UnityEngine;

namespace TestingLab.Gameplay.Spawning
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Pattern:        Observer (listens to WaveSchedule.WaveStarted) + Factory
    //
    // WaveSchedule decides WHEN; this component decides HOW: it instantiates the
    // coin and enemy prefabs. ArenaController drives it with Tick(deltaTime), so
    // spawning stops automatically when the game is paused or over.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class WaveSpawner : MonoBehaviour
    {
        private const float CoinMinDistanceFromPlayer = 2f;
        private const float EnemyMinDistanceFromPlayer = 6f;

        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private GameObject enemyPrefab;

        private WaveSchedule _schedule;
        private SpawnPointPicker _picker;
        private Transform _player;
        private LevelConfig _level;

        public int CoinsSpawned { get; private set; }
        public int EnemiesSpawned { get; private set; }
        public WaveSchedule Schedule => _schedule;

        /// <summary>Raised for every object spawned (handy for tests and effects).</summary>
        public event Action<GameObject> Spawned;

        public void Initialise(WaveSchedule schedule, SpawnPointPicker picker, Transform player, LevelConfig level)
        {
            if (_schedule != null)
                _schedule.WaveStarted -= SpawnWave;

            _schedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
            _picker = picker ?? throw new ArgumentNullException(nameof(picker));
            _player = player;
            _level = level;
            _schedule.WaveStarted += SpawnWave;
        }

        public void Tick(float deltaSeconds) => _schedule?.Advance(deltaSeconds);

        private void OnDestroy()
        {
            if (_schedule != null)
                _schedule.WaveStarted -= SpawnWave;
        }

        private void SpawnWave(WaveDefinition wave, int index)
        {
            for (int i = 0; i < wave.CoinCount; i++)
                SpawnCoin();
            for (int i = 0; i < wave.EnemyCount; i++)
                SpawnEnemy();
        }

        private void SpawnCoin()
        {
            Vector3 position = _picker.Pick(PlayerPosition, CoinMinDistanceFromPlayer);
            var coin = Instantiate(coinPrefab, position, coinPrefab.transform.rotation, transform);
            CoinsSpawned++;
            Spawned?.Invoke(coin);
        }

        private void SpawnEnemy()
        {
            Vector3 position = _picker.Pick(PlayerPosition, EnemyMinDistanceFromPlayer);
            var enemy = Instantiate(enemyPrefab, position, Quaternion.identity, transform);

            if (_level != null)
            {
                if (enemy.TryGetComponent(out EnemyController controller))
                    controller.Configure(_player, _level.EnemySpeed);
                if (enemy.TryGetComponent(out ContactDamage contact))
                    contact.Damage = _level.EnemyContactDamage;
            }

            EnemiesSpawned++;
            Spawned?.Invoke(enemy);
        }

        private Vector3 PlayerPosition => _player != null ? _player.position : Vector3.zero;
    }
}
