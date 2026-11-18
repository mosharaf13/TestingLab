using System;
using TestingLab.Core.Presentation;
using TestingLab.Core.Progression;
using TestingLab.Core.Shop;
using TestingLab.Gameplay.Config;
using TestingLab.Gameplay.Infrastructure;
using TestingLab.Gameplay.UI;
using UnityEngine;

namespace TestingLab.Gameplay.Flow
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED  (end-to-end UI test)
    // Tested by:      Tests/PlayMode/Level3_Advanced/ShopSceneTests.cs
    //
    // Presenter for the Shop scene. Uses the (buggy!) ShopCart, so the bug-hunt
    // bugs are visible in the game:
    //   • Add the same item twice → the cart still says 1.
    //   • Coupon MEGA150 → the total goes negative and checkout throws.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class ShopController : MonoBehaviour
    {
        [SerializeField] private LevelCatalog levels;
        [SerializeField] private ShopCatalog catalog;
        [SerializeField] private ShopView view;

        private readonly CouponBook _coupons = CouponBook.CreateDefault();
        private ShopService _shop;

        public RunProgression Progression { get; private set; }
        public ShopCart Cart { get; } = new ShopCart();

        private void Start()
        {
            Time.timeScale = 1f;
            Progression = GameServices.CreateProgression(levels.Count);
            _shop = new ShopService(catalog);

            view.BuildCatalog(catalog.Items);
            view.AddClicked += OnAdd;
            view.CouponSubmitted += OnCoupon;
            view.CheckoutClicked += OnCheckout;
            view.ClearClicked += OnClear;
            view.LeaveClicked += Progression.LeaveShop;

            view.SetMessage($"Next up: level {Progression.Profile.LevelIndex + 1}. Spend your coins!");
            Refresh();
        }

        private void OnAdd(string sku)
        {
            var item = catalog.Find(sku);
            if (item == null)
                return;

            Cart.AddItem(item.Sku, item.Price);
            view.SetMessage($"Added {item.DisplayName}.");
            Refresh();
        }

        private void OnCoupon(string code)
        {
            if (!_coupons.TryGetPercent(code, out int percent))
            {
                view.SetMessage($"Unknown coupon \"{code}\".");
                return;
            }

            try
            {
                Cart.ApplyCoupon(percent);
                view.SetMessage($"Coupon applied: {percent}% off.");
            }
            catch (ArgumentOutOfRangeException)
            {
                view.SetMessage($"Coupon \"{code}\" is not valid.");
            }
            Refresh();
        }

        private void OnCheckout()
        {
            var result = _shop.Checkout(Cart, Progression.Profile);
            if (result == CheckoutResult.Success)
                Progression.SaveProgress();

            view.SetMessage(ShopFormatter.CheckoutMessage(result));
            Refresh();
        }

        private void OnClear()
        {
            Cart.Clear();
            view.SetMessage("Cart cleared.");
            Refresh();
        }

        private void Refresh()
        {
            view.SetWallet(Progression.Profile.Wallet.Coins);
            view.SetCart(ShopFormatter.Cart(Cart));
            view.SetStats(ShopFormatter.Stats(Progression.Profile.Stats));
        }
    }
}
