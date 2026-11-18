using TestingLab.Core.Progression;
using TestingLab.Core.Shop;

namespace TestingLab.Tests.Shared.Fakes
{
    /// <summary>A SPY effect that counts how many times it was applied.</summary>
    public sealed class SpyItemEffect : IItemEffect
    {
        public int ApplyCount { get; private set; }
        public string Description => "spy effect";

        public void Apply(PlayerStats stats) => ApplyCount++;
    }
}
