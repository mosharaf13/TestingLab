using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TestingLab.Tests.EditMode
{
    // LESSON 5: MonoBehaviours in Edit Mode. Awake/Start/Update do NOT run here,
    // and you must clean up GameObjects yourself with DestroyImmediate.
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
        public void Collector_TryCollect_AddsToInventory()
        {
            var collector = _go.AddComponent<Collector>();
            collector.Configure(5);
            var coin = _go.AddComponent<Collectible>();
            coin.ItemId = "gem";
            coin.Amount = 3;

            bool collected = collector.TryCollect(coin);

            Assert.That(collected, Is.True);
            Assert.That(collector.Inventory.CountOf("gem"), Is.EqualTo(3));
        }
    }
}
