using System;
using System.Collections.Generic;
using TestingLab.Core.Presentation;
using TestingLab.Gameplay.Config;
using UnityEngine;
using UnityEngine.UI;

namespace TestingLab.Gameplay.UI
{
    /// <summary>
    /// Passive View for the Shop. Creates one row per catalog item at runtime.
    /// Buttons are named "Add_&lt;sku&gt;" so Play Mode tests can find and click them.
    /// </summary>
    public sealed class ShopView : MonoBehaviour
    {
        [SerializeField] private RectTransform itemList;
        [SerializeField] private Text walletText;
        [SerializeField] private Text cartText;
        [SerializeField] private Text statsText;
        [SerializeField] private Text messageText;
        [SerializeField] private InputField couponInput;
        [SerializeField] private Button applyCouponButton;
        [SerializeField] private Button checkoutButton;
        [SerializeField] private Button clearButton;
        [SerializeField] private Button leaveButton;

        private readonly List<GameObject> _rows = new List<GameObject>();

        public event Action<string> AddClicked;
        public event Action<string> CouponSubmitted;
        public event Action CheckoutClicked;
        public event Action ClearClicked;
        public event Action LeaveClicked;

        public string CartText => cartText.text;
        public string MessageText => messageText.text;

        private void Awake()
        {
            applyCouponButton.onClick.AddListener(() => CouponSubmitted?.Invoke(couponInput.text));
            checkoutButton.onClick.AddListener(() => CheckoutClicked?.Invoke());
            clearButton.onClick.AddListener(() => ClearClicked?.Invoke());
            leaveButton.onClick.AddListener(() => LeaveClicked?.Invoke());
        }

        public void BuildCatalog(IEnumerable<ShopItemDefinition> items)
        {
            foreach (var row in _rows)
                Destroy(row);
            _rows.Clear();

            foreach (var item in items)
                _rows.Add(CreateRow(item));
        }

        public void SetWallet(int coins) => walletText.text = ShopFormatter.Wallet(coins);

        public void SetCart(string text) => cartText.text = text;

        public void SetStats(string text) => statsText.text = text;

        public void SetMessage(string text) => messageText.text = text;

        private GameObject CreateRow(ShopItemDefinition item)
        {
            var row = UiFactory.CreatePanel(itemList, $"Row_{item.Sku}", UiTheme.Panel);
            UiFactory.AddHorizontalLayout(row.gameObject).padding = new RectOffset(16, 16, 8, 8);
            UiFactory.SetPreferredSize(row, height: 72f);

            string description = ShopFormatter.ItemRow(item.DisplayName, item.CreateEffect().Description, item.Price);
            var label = UiFactory.CreateText(row.transform, "Label", description, UiTheme.BodySize, TextAnchor.MiddleLeft);
            UiFactory.SetPreferredSize(label, flexibleWidth: 1f);

            string sku = item.Sku;
            var add = UiFactory.CreateButton(row.transform, $"Add_{sku}", "Add");
            UiFactory.SetPreferredSize(add, width: 120f);
            add.onClick.AddListener(() => AddClicked?.Invoke(sku));
            return row.gameObject;
        }
    }
}
