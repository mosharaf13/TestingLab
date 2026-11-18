using System.Collections.Generic;
using System.Linq;
using TestingLab.Core.Shop;
using UnityEngine;

namespace TestingLab.Gameplay.Config
{
    /// <summary>What the shop sells. Implements Core's IItemEffectProvider for ShopService.</summary>
    [CreateAssetMenu(menuName = "Testing Lab/Shop Catalog", fileName = "ShopCatalog")]
    public sealed class ShopCatalog : ScriptableObject, IItemEffectProvider
    {
        [SerializeField] private List<ShopItemDefinition> items = new List<ShopItemDefinition>();

        public IReadOnlyList<ShopItemDefinition> Items => items;

        public ShopItemDefinition Find(string sku) => items.FirstOrDefault(i => i.Sku == sku);

        public IItemEffect GetEffect(string sku) => Find(sku)?.CreateEffect();

        public void Configure(IEnumerable<ShopItemDefinition> catalogItems) => items = catalogItems.ToList();
    }
}
