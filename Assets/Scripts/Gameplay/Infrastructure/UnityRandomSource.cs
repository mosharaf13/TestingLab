using TestingLab.Core.Common;

namespace TestingLab.Gameplay.Infrastructure
{
    /// <summary>The real dice. Wraps UnityEngine.Random.</summary>
    public sealed class UnityRandomSource : IRandomSource
    {
        private const float LargestFloatBelowOne = 0.99999994f;

        public float Value01()
        {
            // UnityEngine.Random.value can return exactly 1.0, but the contract says [0, 1).
            float value = UnityEngine.Random.value;
            return value >= 1f ? LargestFloatBelowOne : value;
        }
    }
}
