using System;
using System.Linq;
using TestingLab.Core.Progression;

namespace TestingLab.Core.Shop
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Tested by:      Tests/EditMode/Level2_Intermediate/ShopServiceTests.cs
    // Pattern:        Service / use case. Single Responsibility: it only knows the
    //                 checkout RULES. The cart does maths, the wallet holds money,
    //                 effects change stats, ShopController talks to the UI.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class ShopService
    {
        private readonly IItemEffectProvider _effects;

        public ShopService(IItemEffectProvider effects)
        {
            _effects = effects ?? throw new ArgumentNullException(nameof(effects));
        }

        /// <summary>
        /// Pays for everything in the cart and applies each item's effect once per unit.
        /// Nothing changes unless the whole purchase succeeds.
        /// </summary>
        public CheckoutResult Checkout(ShopCart cart, PlayerProfile profile)
        {
            if (cart == null)
                throw new ArgumentNullException(nameof(cart));
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            if (cart.ItemCount == 0)
                return CheckoutResult.EmptyCart;

            // Look every effect up BEFORE taking money, so a bad SKU can't cost coins.
            var lines = cart.Lines.ToList();
            var effects = lines.Select(l => _effects.GetEffect(l.Sku)).ToList();
            if (effects.Any(e => e == null))
                return CheckoutResult.UnknownItem;

            if (!profile.Wallet.TrySpend(cart.Total))
                return CheckoutResult.NotEnoughCoins;

            for (int i = 0; i < lines.Count; i++)
            {
                for (int unit = 0; unit < lines[i].Quantity; unit++)
                    effects[i].Apply(profile.Stats);
            }

            cart.Clear();
            return CheckoutResult.Success;
        }
    }
}
