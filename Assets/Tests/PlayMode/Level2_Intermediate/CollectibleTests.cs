using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TestingLab.Gameplay.Pickups;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Support;
using UnityEngine;
using UnityEngine.TestTools;

namespace TestingLab.Tests.PlayMode.Level2_Intermediate
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Code under test: Collectible + Collector (coin pickups in the Arena)
    //
    // Topic: PHYSICS in Play Mode. OnTriggerEnter only fires during the physics
    // step, so we wait for FixedUpdate. Also: Unity's "fake null" for destroyed
    // objects (use == null, not Is.Null).
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Intermediate)]
    public class CollectibleTests
    {
        private readonly List<GameObject> _spawned = new List<GameObject>();

        [TearDown]
        public void TearDown()
        {
            foreach (var go in _spawned)
            {
                if (go != null)
                    Object.Destroy(go);
            }
            _spawned.Clear();
        }

        [UnityTest]
        public IEnumerator Collector_TouchingCoin_PicksItUp()
        {
            var player = CreateCollector(Vector3.zero);
            var coin = CreateCoin(Vector3.zero);

            yield return WaitFor.Physics();

            Assert.That(player.Inventory.CountOf(Collectible.CoinId), Is.EqualTo(1));
            // NOTE: Is.Null does NOT work for destroyed Unity objects.
            // Unity overrides ==, so compare with == null instead.
            Assert.That(coin == null, Is.True, "Coin should be destroyed after pickup");
        }

        [UnityTest]
        public IEnumerator Collector_FarFromCoin_DoesNotPickItUp()
        {
            var player = CreateCollector(Vector3.zero);
            var coin = CreateCoin(new Vector3(10f, 0f, 0f));

            yield return WaitFor.Physics();

            Assert.That(player.Inventory.CountOf(Collectible.CoinId), Is.EqualTo(0));
            Assert.That(coin != null, Is.True);
        }

        [UnityTest]
        public IEnumerator Collector_WithFullInventory_LeavesCoinBehind()
        {
            var player = CreateCollector(Vector3.zero);
            player.Configure(1);
            player.Inventory.Add("sword");
            var coin = CreateCoin(Vector3.zero);

            yield return WaitFor.Physics();

            Assert.That(player.Inventory.CountOf(Collectible.CoinId), Is.EqualTo(0));
            Assert.That(coin != null, Is.True, "Coin should still exist");
        }

        private Collector CreateCollector(Vector3 position)
        {
            var go = new GameObject("Player");
            go.transform.position = position;
            go.AddComponent<BoxCollider>();
            var body = go.AddComponent<Rigidbody>();
            body.isKinematic = true;
            _spawned.Add(go);
            return go.AddComponent<Collector>();
        }

        private GameObject CreateCoin(Vector3 position)
        {
            var go = new GameObject("Coin");
            go.transform.position = position;
            var trigger = go.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            go.AddComponent<Collectible>().ItemId = Collectible.CoinId;
            _spawned.Add(go);
            return go;
        }
    }
}
