using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TestingLab.Core.Combat;
using TestingLab.Gameplay.Combat;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Fakes;
using TestingLab.Tests.Shared.Support;
using UnityEngine;
using UnityEngine.TestTools;

namespace TestingLab.Tests.PlayMode.Level2_Intermediate
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE → ADVANCED
    // Code under test: ContactDamage (enemy) + DamageReceiver (player)
    //
    // Topic: mixing REAL physics with FAKE time and randomness.
    // The trigger really fires every physics step, but the invulnerability
    // window uses our FakeTimeProvider, so the test decides when it ends.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Intermediate)]
    public class ContactDamageTests
    {
        private readonly List<GameObject> _spawned = new List<GameObject>();
        private FakeTimeProvider _clock;
        private HealthComponent _playerHealth;

        [SetUp]
        public void SetUp()
        {
            _clock = new FakeTimeProvider();

            var player = Spawn("Player", Vector3.zero);
            player.AddComponent<CapsuleCollider>();
            player.AddComponent<Rigidbody>().isKinematic = true;
            _playerHealth = player.AddComponent<HealthComponent>();
            _playerHealth.SetDestroyOnDeath(false);
            var receiver = player.AddComponent<DamageReceiver>();
            receiver.Construct(new DamageCalculator(new FakeRandomSource { NextValue = 0.99f }), _clock);
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var go in _spawned)
                if (go != null)
                    Object.Destroy(go);
            _spawned.Clear();
        }

        [UnityTest]
        public IEnumerator EnemyTouchingPlayer_DealsDamageOnce_PerInvulnerabilityWindow()
        {
            SpawnEnemy(Vector3.zero, damage: 10);

            yield return WaitFor.Physics();
            Assert.That(_playerHealth.Health.Current, Is.EqualTo(90), "first touch hurts");

            yield return WaitFor.Physics();
            Assert.That(_playerHealth.Health.Current, Is.EqualTo(90), "still invulnerable: fake clock hasn't moved");

            _clock.Advance(1f);
            yield return WaitFor.Physics();
            Assert.That(_playerHealth.Health.Current, Is.EqualTo(80), "window over: hurt again");
        }

        [UnityTest]
        public IEnumerator EnemyFarAway_DealsNoDamage()
        {
            SpawnEnemy(new Vector3(10f, 0f, 0f), damage: 10);

            yield return WaitFor.Physics();

            Assert.That(_playerHealth.Health.Current, Is.EqualTo(100));
        }

        private void SpawnEnemy(Vector3 position, int damage)
        {
            var enemy = Spawn("Enemy", position);
            var trigger = enemy.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            enemy.AddComponent<Rigidbody>().isKinematic = true;
            enemy.AddComponent<ContactDamage>().Damage = damage;
        }

        private GameObject Spawn(string name, Vector3 position)
        {
            var go = new GameObject(name);
            go.transform.position = position;
            _spawned.Add(go);
            return go;
        }
    }
}
