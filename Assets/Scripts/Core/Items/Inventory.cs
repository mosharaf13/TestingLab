using System;
using System.Collections.Generic;
using System.Linq;

namespace TestingLab.Core.Items
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: BEGINNER  ([SetUp], [TestCase], boundary values)
    // Tested by:      Tests/EditMode/Level1_Beginner/InventoryTests.cs
    //                 Tests/EditMode/Level3_Advanced/InventoryPropertyTests.cs
    //
    // Slot-based inventory. Items of the same id stack up to MaxStack per slot.
    // In the game, the player's Collector component owns one of these.
    // ─────────────────────────────────────────────────────────────────────────

    public class Inventory
    {
        private readonly Dictionary<string, int> _items = new Dictionary<string, int>();

        public int SlotCapacity { get; }
        public int MaxStack { get; }
        public IReadOnlyDictionary<string, int> Items => _items;
        public int UsedSlots => _items.Values.Sum(SlotsFor);

        public Inventory(int slotCapacity, int maxStack = 99)
        {
            if (slotCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(slotCapacity));
            if (maxStack <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxStack));

            SlotCapacity = slotCapacity;
            MaxStack = maxStack;
        }

        public int CountOf(string itemId)
        {
            return itemId != null && _items.TryGetValue(itemId, out int count) ? count : 0;
        }

        /// <returns>True if the items fit; false (and no change) if they don't.</returns>
        public bool Add(string itemId, int count = 1)
        {
            if (string.IsNullOrWhiteSpace(itemId))
                throw new ArgumentException("Item id is required.", nameof(itemId));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            int current = CountOf(itemId);
            int newCount = current + count;
            int slotsAfter = UsedSlots - SlotsFor(current) + SlotsFor(newCount);
            if (slotsAfter > SlotCapacity)
                return false;

            _items[itemId] = newCount;
            return true;
        }

        /// <returns>True if removed; false (and no change) if there weren't enough.</returns>
        public bool Remove(string itemId, int count = 1)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            int have = CountOf(itemId);
            if (have < count)
                return false;

            if (have == count)
                _items.Remove(itemId);
            else
                _items[itemId] = have - count;
            return true;
        }

        private int SlotsFor(int count) => (count + MaxStack - 1) / MaxStack;
    }
}
