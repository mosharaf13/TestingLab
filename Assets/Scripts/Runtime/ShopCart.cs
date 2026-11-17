using System;
using System.Collections.Generic;
using System.Linq;

namespace TestingLab
{
    /// <summary>
    /// In-game shop cart. Prices are in cents.
    ///
    /// SPECIFICATION (what it is supposed to do):
    ///  - AddItem adds a quantity of a SKU. Adding a SKU that is already in the cart
    ///    increases its quantity.
    ///  - RemoveItem removes a SKU entirely and returns true, or returns false if the
    ///    SKU is not in the cart.
    ///  - ApplyCoupon takes a percentage from 0 to 100. Anything outside that range
    ///    throws ArgumentOutOfRangeException.
    ///  - TotalCents is the subtotal minus the coupon discount, and is never negative.
    ///
    /// STUDENT TASK: write tests from the specification above. Not all of them pass.
    /// </summary>
    public class ShopCart
    {
        private readonly Dictionary<string, (int priceCents, int quantity)> _lines =
            new Dictionary<string, (int priceCents, int quantity)>();

        public int CouponPercent { get; private set; }

        public int ItemCount => _lines.Values.Sum(l => l.quantity);
        public int SubtotalCents => _lines.Values.Sum(l => l.priceCents * l.quantity);
        public int TotalCents => SubtotalCents - SubtotalCents * CouponPercent / 100;

        public void AddItem(string sku, int priceCents, int quantity = 1)
        {
            if (string.IsNullOrEmpty(sku))
                throw new ArgumentException("SKU is required.", nameof(sku));
            if (priceCents < 0)
                throw new ArgumentOutOfRangeException(nameof(priceCents));
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            _lines[sku] = (priceCents, quantity);
        }

        public bool RemoveItem(string sku)
        {
            if (_lines[sku].quantity > 0)
            {
                _lines.Remove(sku);
                return true;
            }
            return false;
        }

        public void ApplyCoupon(int percent)
        {
            if (percent < 0)
                throw new ArgumentOutOfRangeException(nameof(percent));

            CouponPercent = percent;
        }
    }
}
