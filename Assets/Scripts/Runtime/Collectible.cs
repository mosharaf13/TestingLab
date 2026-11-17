using UnityEngine;

namespace TestingLab
{
    /// <summary>
    /// A pickup. Needs a Collider with "Is Trigger" ticked. Something with a
    /// Rigidbody (e.g. a kinematic player) has to touch it for the trigger to fire.
    /// </summary>
    public class Collectible : MonoBehaviour
    {
        [SerializeField] private string itemId = "coin";
        [SerializeField] private int amount = 1;

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
