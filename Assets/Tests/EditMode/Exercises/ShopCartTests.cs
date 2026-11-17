using NUnit.Framework;

namespace TestingLab.Tests.EditMode.Exercises
{
    // BUG HUNT: read the SPECIFICATION at the top of ShopCart.cs, then write
    // tests for each rule. A correct test that FAILS means you found a bug.
    // Remove the [Ignore] attribute when you start a test.
    public class ShopCartTests
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
            _cart.AddItem("sword", priceCents: 1500);

            Assert.That(_cart.SubtotalCents, Is.EqualTo(1500));
        }

        [Test, Ignore("Exercise: adding the same SKU twice should sum the quantities")]
        public void AddItem_SameSkuTwice_QuantitiesAreSummed()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise: removing a SKU that is not in the cart should return false")]
        public void RemoveItem_UnknownSku_ReturnsFalse()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise: a coupon above 100% should throw ArgumentOutOfRangeException")]
        public void ApplyCoupon_Over100_Throws()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise: a 25% coupon on 2000 cents should give a total of 1500")]
        public void ApplyCoupon_25Percent_ReducesTotal()
        {
            Assert.Fail("TODO");
        }

        [Test, Ignore("Exercise: the total should never be negative")]
        public void Total_IsNeverNegative()
        {
            Assert.Fail("TODO");
        }
    }
}
