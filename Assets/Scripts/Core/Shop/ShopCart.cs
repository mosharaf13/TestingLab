using System;
using System.Collections.Generic;
using System.Linq;

namespace TestingLab.Core.Shop
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE  ← BUG HUNT
    // Exercise:       Tests/EditMode/Exercises/Level2_Intermediate/ShopCartBugHuntTests.cs
    //
    // ⚠ THIS CLASS HAS DELIBERATE BUGS. Don't fix them by reading the code.
    //   Write a test from the SPECIFICATION below. A correct test that FAILS
    //   means you found a bug. Then fix the code and watch the test turn green.
    //
    // You can also SEE the bugs in the game: open the Shop scene, add two of the
    // same item, or try the coupon code MEGA150.
    //
    // SPECIFICATION (what it is supposed to do). Prices are in coins.
    //  - AddItem adds a quantity of a SKU. Adding a SKU that is already in the
    //    cart increases its quantity.
    //  - RemoveItem removes a SKU entirely and returns true, or returns false if
    //    the SKU is not in the cart.
    //  - ApplyCoupon takes a percentage from 0 to 100. Anything outside that range
    //    throws ArgumentOutOfRangeException.
    //  - Total is the subtotal minus the coupon discount, and is never negative.
    //  - Clear empties the cart and removes the coupon.
    // ─────────────────────────────────────────────────────────────────────────

    public class ShopCart
    {
        private readonly Dictionary<string, CartLine> _lines = new Dictionary<string, CartLine>();

        public int CouponPercent { get; private set; }

        public IEnumerable<CartLine> Lines => _lines.Values;
        public int ItemCount => _lines.Values.Sum(l => l.Quantity);
        public int Subtotal => _lines.Values.Sum(l => l.LineTotal);
        public int Total => Subtotal - Subtotal * CouponPercent / 100;

        public int QuantityOf(string sku) =>
            sku != null && _lines.TryGetValue(sku, out var line) ? line.Quantity : 0;

        public void AddItem(string sku, int price, int quantity = 1)
        {
            if (string.IsNullOrEmpty(sku))
                throw new ArgumentException("SKU is required.", nameof(sku));
            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(price));
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            _lines[sku] = new CartLine(sku, price, quantity);
        }

        public bool RemoveItem(string sku)
        {
            if (_lines[sku].Quantity > 0)
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

        public void Clear()
        {
            _lines.Clear();
            CouponPercent = 0;
        }
    }
}
