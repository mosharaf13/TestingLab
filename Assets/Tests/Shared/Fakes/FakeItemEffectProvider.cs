using System.Collections.Generic;
using TestingLab.Core.Shop;

namespace TestingLab.Tests.Shared.Fakes
{
    /// <summary>A FAKE catalog: a dictionary from SKU to effect.</summary>
    public sealed class FakeItemEffectProvider : IItemEffectProvider
    {
        private readonly Dictionary<string, IItemEffect> _effects = new Dictionary<string, IItemEffect>();

        public FakeItemEffectProvider With(string sku, IItemEffect effect)
        {
            _effects[sku] = effect;
            return this;
        }

        public IItemEffect GetEffect(string sku) => _effects.TryGetValue(sku, out var effect) ? effect : null;
    }
}
