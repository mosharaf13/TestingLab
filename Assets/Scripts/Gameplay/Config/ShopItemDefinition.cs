using System;
using TestingLab.Core.Shop;
using UnityEngine;

namespace TestingLab.Gameplay.Config
{
    // Pattern: Factory Method. Unity can't serialize an interface field, so the
    // Inspector stores an enum + number and CreateEffect() turns that into the
    // right IItemEffect strategy. This switch is the ONLY place that maps kinds
    // to classes; adding a kind means one new enum value and one new case.

    [Serializable]
    public sealed class ShopItemDefinition
    {
        [SerializeField] private string sku;
        [SerializeField] private string displayName;
        [SerializeField, Min(0)] private int price;
        [SerializeField] private ItemEffectKind effect;
        [SerializeField, Min(0f)] private float amount;

        public ShopItemDefinition(string sku, string displayName, int price, ItemEffectKind effect, float amount)
        {
            this.sku = sku;
            this.displayName = displayName;
            this.price = price;
            this.effect = effect;
            this.amount = amount;
        }

        public string Sku => sku;
        public string DisplayName => displayName;
        public int Price => price;
        public ItemEffectKind Effect => effect;
        public float Amount => amount;

        public IItemEffect CreateEffect() => effect switch
        {
            ItemEffectKind.MaxHealth => new MaxHealthBoost(Mathf.RoundToInt(amount)),
            ItemEffectKind.Armor => new ArmorBoost(Mathf.RoundToInt(amount)),
            ItemEffectKind.MoveSpeed => new SpeedBoost(amount),
            _ => throw new ArgumentOutOfRangeException(nameof(effect), effect, "Unknown item effect.")
        };
    }
}
