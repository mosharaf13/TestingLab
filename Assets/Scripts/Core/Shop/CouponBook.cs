using System;
using System.Collections.Generic;

namespace TestingLab.Core.Shop
{
    // Learning level: BEGINNER
    // Tested by: Tests/EditMode/Level1_Beginner/CouponBookTests.cs
    //
    // Maps coupon codes typed in the Shop to a discount percentage. Codes are
    // case-insensitive and surrounding spaces are ignored.
    //
    // Story for the bug hunt: a designer meant to add "MEGA15" but typed "MEGA150".
    // A correct ShopCart should reject a 150% coupon. Does it?

    public sealed class CouponBook
    {
        private readonly Dictionary<string, int> _percentByCode =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        public static CouponBook CreateDefault()
        {
            var book = new CouponBook();
            book.Register("LAB10", 10);
            book.Register("LAB25", 25);
            book.Register("MEGA150", 150);
            return book;
        }

        public void Register(string code, int percent)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Coupon code is required.", nameof(code));

            _percentByCode[code.Trim()] = percent;
        }

        public bool TryGetPercent(string code, out int percent)
        {
            percent = 0;
            return !string.IsNullOrWhiteSpace(code) && _percentByCode.TryGetValue(code.Trim(), out percent);
        }
    }
}
