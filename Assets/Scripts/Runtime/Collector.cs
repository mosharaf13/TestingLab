using System;
using UnityEngine;

namespace TestingLab
{
    /// <summary>
    /// Anything that can pick up Collectibles (usually the player).
    /// </summary>
    public class Collector : MonoBehaviour
    {
        [SerializeField] private int slots = 10;

        public Inventory Inventory { get; private set; }

        /// <summary>(itemId, amount)</summary>
        public event Action<string, int> Collected;

        private void Awake()
        {
            if (Inventory == null)
                Inventory = new Inventory(slots);
        }

        /// <summary>
        /// Awake does not run in Edit Mode tests, so tests call this directly.
        /// </summary>
        public void Configure(int slotCount)
        {
            slots = slotCount;
            Inventory = new Inventory(slotCount);
        }

        public bool TryCollect(Collectible collectible)
        {
            if (collectible == null)
                return false;
            if (!Inventory.Add(collectible.ItemId, collectible.Amount))
                return false;

            Debug.Log($"Collected {collectible.Amount}x {collectible.ItemId} (now {Inventory.CountOf(collectible.ItemId)})");
            Collected?.Invoke(collectible.ItemId, collectible.Amount);
            return true;
        }
    }
}
