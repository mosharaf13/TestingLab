using TestingLab.Core.Progression;

namespace TestingLab.Core.Shop
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Pattern:        Strategy + Open/Closed Principle (the "O" in SOLID)
    // Tested by:      Tests/EditMode/Level2_Intermediate/ItemEffectTests.cs
    //
    // Every shop item knows how to apply itself to the player's stats. To add a new
    // item you write a NEW class; you never edit ShopService. ShopService is
    // "closed for modification, open for extension".
    // ─────────────────────────────────────────────────────────────────────────

    public interface IItemEffect
    {
        /// <summary>Short text for the shop UI, e.g. "+20 max health".</summary>
        string Description { get; }

        void Apply(PlayerStats stats);
    }
}
