using System;
using NUnit.Framework;

namespace TestingLab.Tests.EditMode
{
    // LESSON 3: SetUp/TearDown and parameterised tests ([TestCase], ExpectedResult).
    public class InventoryTests
    {
        private Inventory _inventory;

        [SetUp]
        public void SetUp()
        {
            // Runs before EVERY test, so each test gets a fresh inventory.
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

        [TestCase(1, ExpectedResult = 1)]
        [TestCase(10, ExpectedResult = 1)]
        [TestCase(11, ExpectedResult = 2)]
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
