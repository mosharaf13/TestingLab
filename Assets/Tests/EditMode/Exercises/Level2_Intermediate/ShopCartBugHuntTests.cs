using NUnit.Framework;
using TestingLab.Core.Shop;
using TestingLab.Tests.Shared;

namespace TestingLab.Tests.EditMode.Exercises.Level2_Intermediate
{
    // ═════════════════════════════════════════════════════════════════════════
    //  BUG HUNT — Learning level: INTERMEDIATE
    //
    //  1. Play the game and visit the Shop. Add the same item twice. Try the
    //     coupon MEGA150. Something is wrong!
    //  2. Read the SPECIFICATION at the top of Assets/Scripts/Core/Shop/ShopCart.cs.
    //  3. Write one test per rule below. A correct test that FAILS = a bug found.
    //  4. Only THEN fix ShopCart. Watch your test go green. The test now guards
    //     against the bug ever coming back (a "regression test").
    //
    //  Remove the [Ignore] attribute when you start a test.
    // ═════════════════════════════════════════════════════════════════════════

    [Category(TestCategory.Intermediate), Category(TestCategory.Exercise)]
    public class ShopCartBugHuntTests
    {
        private ShopCart _cart;

        [SetUp]
        public void SetUp()
        {
            _cart = new ShopCart();
        }

        // Worked example: this one is done for you.
        [Test]
        public void AddItem_OneItem_SubtotalEqualsPrice()
        {
            _cart.AddItem("sword", price: 15);

            Assert.That(_cart.Subtotal, Is.EqualTo(15));
        }

        [Test, Ignore("Bug hunt: adding the same SKU twice should sum the quantities")]
        public void AddItem_SameSkuTwice_QuantitiesAreSummed()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Bug hunt: removing a SKU that is not in the cart should return false")]
        public void RemoveItem_UnknownSku_ReturnsFalse()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Bug hunt: a coupon above 100% should throw ArgumentOutOfRangeException")]
        public void ApplyCoupon_Over100_Throws()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Bug hunt: a 25% coupon on 20 coins should give a total of 15")]
        public void ApplyCoupon_25Percent_ReducesTotal()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Bug hunt: the total should never be negative")]
        public void Total_IsNeverNegative()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Bug hunt: Clear() empties the cart AND removes the coupon")]
        public void Clear_RemovesItemsAndCoupon()
        {
            Assert.Fail("TODO");
        }
    }
}
