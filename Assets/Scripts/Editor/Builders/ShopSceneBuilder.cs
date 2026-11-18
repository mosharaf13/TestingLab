using TestingLab.Gameplay.Config;
using TestingLab.Gameplay.Flow;
using TestingLab.Gameplay.Infrastructure;
using TestingLab.Gameplay.UI;
using UnityEngine;

namespace TestingLab.EditorTools.Builders
{
    internal static class ShopSceneBuilder
    {
        public static void Build()
        {
            var scene = SceneBuilderBase.NewScene();
            var levels = SceneBuilderBase.Load<LevelCatalog>(ContentPaths.LevelCatalog);
            var catalog = SceneBuilderBase.Load<ShopCatalog>(ContentPaths.ShopCatalog);

            var canvas = UiFactory.CreateCanvas("Shop Canvas");
            UiFactory.EnsureEventSystem();

            var title = UiFactory.CreateText(canvas.transform, "Title", "SHOP", UiTheme.TitleSize, color: UiTheme.Accent);
            title.fontStyle = FontStyle.Bold;
            UiFactory.Place(title.rectTransform, new Vector2(0.5f, 1f), new Vector2(800f, 100f), new Vector2(0f, -24f));

            var message = UiFactory.CreateText(canvas.transform, "Message", "", UiTheme.BodySize, color: UiTheme.MutedText);
            UiFactory.Place(message.rectTransform, new Vector2(0.5f, 1f), new Vector2(1400f, 50f), new Vector2(0f, -124f));

            // Left: what's for sale (rows are created at runtime by ShopView).
            var items = UiFactory.CreatePanel(canvas.transform, "Items", UiTheme.Background);
            UiFactory.Place(items.rectTransform, new Vector2(0f, 0.5f), new Vector2(980f, 640f), new Vector2(60f, -20f));
            UiFactory.AddVerticalLayout(items.gameObject, spacing: 12f, padding: 24);
            SceneBuilderBase.Heading(items.transform, "Heading", "For sale");
            var itemList = UiFactory.CreateRect(items.transform, "Item List");
            UiFactory.AddVerticalLayout(itemList.gameObject, spacing: 12f, padding: 0);

            // Right: wallet, cart, coupon, actions.
            var side = UiFactory.CreatePanel(canvas.transform, "Cart", UiTheme.Background);
            UiFactory.Place(side.rectTransform, new Vector2(1f, 0.5f), new Vector2(760f, 760f), new Vector2(-60f, -20f));
            UiFactory.AddVerticalLayout(side.gameObject, spacing: 12f, padding: 24);
            var wallet = SceneBuilderBase.Heading(side.transform, "Wallet", "Wallet: 0 coins");
            wallet.color = UiTheme.Accent;
            var cart = UiFactory.CreateText(side.transform, "Cart Text", "Cart is empty", UiTheme.BodySize, TextAnchor.UpperLeft);
            UiFactory.SetPreferredSize(cart, height: 200f);

            var couponRow = UiFactory.CreateRect(side.transform, "Coupon Row");
            UiFactory.AddHorizontalLayout(couponRow.gameObject);
            UiFactory.SetPreferredSize(couponRow, height: 56f);
            var coupon = UiFactory.CreateInputField(couponRow, "CouponInput", "Coupon code (try LAB10)");
            UiFactory.SetPreferredSize(coupon, flexibleWidth: 1f);
            var apply = UiFactory.CreateButton(couponRow, "ApplyCouponButton", "Apply");
            UiFactory.SetPreferredSize(apply, width: 140f);

            var checkout = UiFactory.CreateButton(side.transform, "CheckoutButton", "Checkout");
            var clear = UiFactory.CreateButton(side.transform, "ClearButton", "Clear cart", UiTheme.DangerButton);
            var stats = UiFactory.CreateText(side.transform, "Stats", "", UiTheme.SmallSize, TextAnchor.UpperLeft, UiTheme.MutedText);
            UiFactory.SetPreferredSize(stats, height: 90f);

            var leave = UiFactory.CreateButton(canvas.transform, "LeaveButton", "Next level  ▶");
            UiFactory.Place((RectTransform)leave.transform, new Vector2(0.5f, 0f), new Vector2(420f, 72f), new Vector2(0f, 40f));

            var view = new GameObject("ShopView").AddComponent<ShopView>();
            Wiring.Set(view, "itemList", itemList);
            Wiring.Set(view, "walletText", wallet);
            Wiring.Set(view, "cartText", cart);
            Wiring.Set(view, "statsText", stats);
            Wiring.Set(view, "messageText", message);
            Wiring.Set(view, "couponInput", coupon);
            Wiring.Set(view, "applyCouponButton", apply);
            Wiring.Set(view, "checkoutButton", checkout);
            Wiring.Set(view, "clearButton", clear);
            Wiring.Set(view, "leaveButton", leave);

            var controller = new GameObject("ShopController").AddComponent<ShopController>();
            Wiring.Set(controller, "levels", levels);
            Wiring.Set(controller, "catalog", catalog);
            Wiring.Set(controller, "view", view);

            SceneBuilderBase.AddLabNotes("Shop", LabNotesText.Shop);
            SceneBuilderBase.Save(scene, SceneNames.Shop);
        }
    }
}
