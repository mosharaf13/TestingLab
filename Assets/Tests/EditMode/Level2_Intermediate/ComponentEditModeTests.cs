using NUnit.Framework;
using TestingLab.Core.Combat;
using TestingLab.Gameplay.Combat;
using TestingLab.Gameplay.Pickups;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Fakes;
using UnityEngine;
using UnityEngine.TestTools;

namespace TestingLab.Tests.EditMode.Level2_Intermediate
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Topic: MonoBehaviours in EDIT MODE.
    //   • Awake/Start/Update do NOT run here, so call setup methods yourself.
    //   • Destroy() isn't allowed; clean up with DestroyImmediate in [TearDown].
    //   • Use this for components whose methods you can call directly. If you need
    //     frames, physics or coroutines, use a Play Mode test instead.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Intermediate)]
    public class ComponentEditModeTests
    {
        private GameObject _go;

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject("Dummy");
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_go);
        }

        [Test]
        public void HealthComponent_OnDeath_LogsMessage()
        {
            var component = _go.AddComponent<HealthComponent>();
            component.SetDestroyOnDeath(false); // Destroy() is not allowed in Edit Mode
            component.Initialise(10);            // Awake did not run, so set it up by hand

            LogAssert.Expect(LogType.Log, "Dummy died");
            component.Health.TakeDamage(10);
        }

        [Test]
        public void Collector_TryCollect_AddsToInventoryAndRaisesEvent()
        {
            var collector = _go.AddComponent<Collector>();
            collector.Configure(5);
            var gem = _go.AddComponent<Collectible>();
            gem.ItemId = "gem";
            gem.Amount = 3;
            string collectedId = null;
            collector.Collected += (id, amount) => collectedId = id;

            bool collected = collector.TryCollect(gem);

            Assert.That(collected, Is.True);
            Assert.That(collector.Inventory.CountOf("gem"), Is.EqualTo(3));
            Assert.That(collectedId, Is.EqualTo("gem"));
        }

        [Test]
        public void DamageReceiver_UsesArmorAndInvulnerabilityWindow()
        {
            // Method injection: give the component fakes instead of real dice and clock.
            var clock = new FakeTimeProvider();
            var receiver = _go.AddComponent<DamageReceiver>(); // RequireComponent adds HealthComponent too
            var health = _go.GetComponent<HealthComponent>();
            health.Initialise(100);
            receiver.Armor = 100; // halves damage
            receiver.Construct(new DamageCalculator(new FakeRandomSource { NextValue = 0.99f }), clock);

            bool firstHit = receiver.TryReceiveHit(20);
            bool secondHit = receiver.TryReceiveHit(20); // same instant: still invulnerable
            clock.Advance(1f);
            bool thirdHit = receiver.TryReceiveHit(20);

            Assert.That(new[] { firstHit, secondHit, thirdHit }, Is.EqualTo(new[] { true, false, true }));
            Assert.That(health.Health.Current, Is.EqualTo(80));
        }

        [Test]
        public void DamageReceiver_GodMode_IgnoresHits()
        {
            var receiver = _go.AddComponent<DamageReceiver>();
            _go.GetComponent<HealthComponent>().Initialise(100);
            receiver.GodMode = true;

            Assert.That(receiver.TryReceiveHit(50), Is.False);
        }
    }
}
