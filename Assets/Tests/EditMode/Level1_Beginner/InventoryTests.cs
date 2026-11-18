using System;
using NUnit.Framework;
using TestingLab.Core.Items;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Level1_Beginner
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: BEGINNER
    // Code under test: Assets/Scripts/Core/Items/Inventory.cs
    // In the game:     the player's Collector stores picked-up coins here.
    //
    // Topics: [SetUp] for shared arrange code, [TestCase] with ExpectedResult,
    //         boundary values (10 fits in one stack, 11 needs two).
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Beginner)]
    public class InventoryTests
    {
        private Inventory _inventory;

        [SetUp]
        public void SetUp()
        {
            // Runs before EVERY test, so each test gets a fresh inventory.
            // Tests must never depend on each other or on running order.
            _inventory = new Inventory(slotCapacity: 3, maxStack: 10);
        }

        [Test]
        public void Add_NewItem_IsCounted()
        {
            bool added = _inventory.Add("potion", 2);

            Assert.That(added, Is.True);
            Assert.That(_inventory.CountOf("potion"), Is.EqualTo(2));
        }

        [Test]
        public void Add_SameItemTwice_Stacks()
        {
            _inventory.Add("arrow", 4);
            _inventory.Add("arrow", 3);

            Assert.That(_inventory.CountOf("arrow"), Is.EqualTo(7));
            Assert.That(_inventory.UsedSlots, Is.EqualTo(1));
        }

        // ExpectedResult: the test RETURNS a value and NUnit compares it for you.
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(10, ExpectedResult = 1)]   // boundary: exactly one full stack
        [TestCase(11, ExpectedResult = 2)]   // boundary + 1
        [TestCase(30, ExpectedResult = 3)]
        public int UsedSlots_DependsOnStackSize(int count)
        {
            _inventory.Add("arrow", count);
            return _inventory.UsedSlots;
        }

        [Test]
        public void Add_WhenFull_ReturnsFalse_AndChangesNothing()
        {
            _inventory.Add("sword");
            _inventory.Add("shield");
            _inventory.Add("helmet");

            bool added = _inventory.Add("boots");

            Assert.That(added, Is.False);
            Assert.That(_inventory.CountOf("boots"), Is.EqualTo(0));
            Assert.That(_inventory.UsedSlots, Is.EqualTo(3));
        }

        [Test]
        public void Add_OverflowingAStack_FailsIfNoSlotIsFree()
        {
            _inventory.Add("arrow", 10);
            _inventory.Add("sword");
            _inventory.Add("shield");

            Assert.That(_inventory.Add("arrow", 1), Is.False);
            Assert.That(_inventory.CountOf("arrow"), Is.EqualTo(10));
        }

        [Test]
        public void Remove_AllOfAnItem_FreesTheSlot()
        {
            _inventory.Add("potion", 3);

            bool removed = _inventory.Remove("potion", 3);

            Assert.That(removed, Is.True);
            Assert.That(_inventory.Items.ContainsKey("potion"), Is.False);
            Assert.That(_inventory.UsedSlots, Is.EqualTo(0));
        }

        [TestCase((string)null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Add_InvalidId_Throws(string itemId)
        {
            Assert.Throws<ArgumentException>(() => _inventory.Add(itemId));
        }
    }
}
