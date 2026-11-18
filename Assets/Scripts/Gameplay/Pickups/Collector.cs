using System;
using TestingLab.Core.Items;
using UnityEngine;

namespace TestingLab.Gameplay.Pickups
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Tested by:      Tests/EditMode/Level2_Intermediate/ComponentEditModeTests.cs
    //                 Tests/PlayMode/Level2_Intermediate/CollectibleTests.cs
    //
    // Anything that can pick up Collectibles (the player). Raises Collected so
    // other systems (score, session) can react — Observer pattern. The Collector
    // doesn't know or care who is listening.
    // ─────────────────────────────────────────────────────────────────────────

    public class Collector : MonoBehaviour
    {
        [SerializeField, Min(1)] private int slots = 10;

        public Inventory Inventory { get; private set; }

        /// <summary>(itemId, amount)</summary>
        public event Action<string, int> Collected;

        private void Awake()
        {
            if (Inventory == null)
                Inventory = new Inventory(slots);
        }

        /// <summary>Awake does not run in Edit Mode tests, so tests call this directly.</summary>
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

            Collected?.Invoke(collectible.ItemId, collectible.Amount);
            return true;
        }
    }
}
