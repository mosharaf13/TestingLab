using UnityEngine;

namespace TestingLab.Gameplay.Pickups
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE  (physics in Play Mode)
    // Tested by:      Tests/PlayMode/Level2_Intermediate/CollectibleTests.cs
    //
    // A pickup. Needs a Collider with "Is Trigger" ticked. Something with a
    // Rigidbody (the kinematic player) has to touch it for the trigger to fire.
    // ─────────────────────────────────────────────────────────────────────────

    public class Collectible : MonoBehaviour
    {
        public const string CoinId = "coin";

        [SerializeField] private string itemId = CoinId;
        [SerializeField, Min(1)] private int amount = 1;

        public string ItemId
        {
            get => itemId;
            set => itemId = value;
        }

        public int Amount
        {
            get => amount;
            set => amount = value;
        }

        private void OnTriggerEnter(Collider other)
        {
            var collector = other.GetComponentInParent<Collector>();
            if (collector != null && collector.TryCollect(this))
                Destroy(gameObject);
        }
    }
}
