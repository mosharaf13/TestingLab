using System;

namespace TestingLab.Core.Common
{
    // Learning level: ADVANCED (used by the seeded "property-based" tests)
    //
    // A real random source built on System.Random. Give it a seed and it produces
    // the SAME sequence every time, which lets a test throw thousands of random
    // operations at a class and still be repeatable when it fails.

    public sealed class SystemRandomSource : IRandomSource
    {
        private const float LargestFloatBelowOne = 0.99999994f;
        private readonly Random _random;

        public SystemRandomSource(int seed) => _random = new Random(seed);

        public float Value01()
        {
            // NextDouble() is < 1, but casting 0.9999999999 to float can round UP to 1.0f.
            float value = (float)_random.NextDouble();
            return value >= 1f ? LargestFloatBelowOne : value;
        }
    }
}
