namespace TestingLab.Core.Shop
{
    // Pattern: Dependency Inversion. ShopService needs "the effect for this SKU" but
    // must not know WHERE effects come from. In the game a ShopCatalog
    // ScriptableObject answers; in tests a tiny fake dictionary does.

    public interface IItemEffectProvider
    {
        /// <returns>The effect, or null if the SKU is unknown.</returns>
        IItemEffect GetEffect(string sku);
    }
}
