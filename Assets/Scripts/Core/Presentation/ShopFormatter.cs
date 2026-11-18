using System.Linq;
using System.Text;
using TestingLab.Core.Progression;
using TestingLab.Core.Shop;

namespace TestingLab.Core.Presentation
{
    // Learning level: BEGINNER
    // Text for the Shop screen. Kept out of ShopView so it can be unit tested.

    public static class ShopFormatter
    {
        public static string Wallet(int coins) => $"Wallet: {coins} coins";

        public static string ItemRow(string displayName, string effectDescription, int price) =>
            $"{displayName}  ({effectDescription})  {price}c";

        public static string Cart(ShopCart cart)
        {
            if (cart.ItemCount == 0)
                return "Cart is empty";

            var text = new StringBuilder();
            foreach (var line in cart.Lines.OrderBy(l => l.Sku))
                text.AppendLine($"{line.Quantity} x {line.Sku}  = {line.LineTotal}c");

            text.AppendLine($"Subtotal {cart.Subtotal}c");
            if (cart.CouponPercent > 0)
                text.AppendLine($"Coupon -{cart.CouponPercent}%");
            text.Append($"TOTAL {cart.Total}c");
            return text.ToString();
        }

        public static string Stats(PlayerStats stats) =>
            $"Max health {stats.MaxHealth}\nArmor {stats.Armor}\nSpeed {stats.MoveSpeed:0.#}";

        public static string CheckoutMessage(CheckoutResult result) => result switch
        {
            CheckoutResult.Success => "Purchased! Upgrades applied.",
            CheckoutResult.EmptyCart => "Your cart is empty.",
            CheckoutResult.NotEnoughCoins => "Not enough coins.",
            CheckoutResult.UnknownItem => "Something in the cart isn't sold here.",
            _ => result.ToString()
        };
    }
}
