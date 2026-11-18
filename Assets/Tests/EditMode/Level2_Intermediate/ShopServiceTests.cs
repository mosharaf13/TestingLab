using NUnit.Framework;
using TestingLab.Core.Progression;
using TestingLab.Core.Shop;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Builders;
using TestingLab.Tests.Shared.Fakes;

namespace TestingLab.Tests.EditMode.Level2_Intermediate
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Code under test: Assets/Scripts/Core/Shop/ShopService.cs
    // In the game:     the Checkout button in the Shop.
    //
    // Topics: fakes (FakeItemEffectProvider) and spies (SpyItemEffect) working
    //         together; testing that a FAILED operation changes NOTHING.
    //
    // Note: these tests avoid the known ShopCart bugs (one SKU per AddItem call)
    // so they test ShopService, not ShopCart. One test, one reason to fail.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Intermediate)]
    public class ShopServiceTests
    {
        private SpyItemEffect _armorEffect;
        private ShopService _shop;
        private ShopCart _cart;

        [SetUp]
        public void SetUp()
        {
            _armorEffect = new SpyItemEffect();
            var effects = new FakeItemEffectProvider().With("armor", _armorEffect);
            _shop = new ShopService(effects);
            _cart = new ShopCart();
        }

        [Test]
        public void Checkout_WithEnoughCoins_PaysAppliesEffectAndEmptiesCart()
        {
            PlayerProfile profile = A.Profile().WithCoins(50);
            _cart.AddItem("armor", price: 20, quantity: 2);

            var result = _shop.Checkout(_cart, profile);

            Assert.That(result, Is.EqualTo(CheckoutResult.Success));
            Assert.That(profile.Wallet.Coins, Is.EqualTo(10));
            Assert.That(_armorEffect.ApplyCount, Is.EqualTo(2), "once per unit bought");
            Assert.That(_cart.ItemCount, Is.EqualTo(0));
        }

        [Test]
        public void Checkout_NotEnoughCoins_ChangesNothing()
        {
            PlayerProfile profile = A.Profile().WithCoins(5);
            _cart.AddItem("armor", price: 20);

            var result = _shop.Checkout(_cart, profile);

            Assert.That(result, Is.EqualTo(CheckoutResult.NotEnoughCoins));
            Assert.That(profile.Wallet.Coins, Is.EqualTo(5));
            Assert.That(_armorEffect.ApplyCount, Is.EqualTo(0));
            Assert.That(_cart.ItemCount, Is.EqualTo(1), "the cart is kept so the player can adjust it");
        }

        [Test]
        public void Checkout_EmptyCart_IsRejected()
        {
            var result = _shop.Checkout(_cart, A.Profile().WithCoins(50));

            Assert.That(result, Is.EqualTo(CheckoutResult.EmptyCart));
        }

        [Test]
        public void Checkout_UnknownSku_TakesNoMoney()
        {
            PlayerProfile profile = A.Profile().WithCoins(50);
            _cart.AddItem("mystery", price: 10);

            var result = _shop.Checkout(_cart, profile);

            Assert.That(result, Is.EqualTo(CheckoutResult.UnknownItem));
            Assert.That(profile.Wallet.Coins, Is.EqualTo(50));
        }

        [Test]
        public void Checkout_UsesTheDiscountedTotal()
        {
            PlayerProfile profile = A.Profile().WithCoins(100);
            _cart.AddItem("armor", price: 40);
            _cart.ApplyCoupon(25);

            _shop.Checkout(_cart, profile);

            Assert.That(profile.Wallet.Coins, Is.EqualTo(70));
        }
    }
}
