using System;

namespace TestingLab.Core.Common
{
    // Learning level: BEGINNER
    // Extension methods add helpers to an interface without making every
    // implementation write them again (Open/Closed Principle: IRandomSource stays tiny).

    public static class RandomExtensions
    {
        /// <summary>A float in [min, max).</summary>
        public static float Range(this IRandomSource random, float min, float max)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));
            if (max < min)
                throw new ArgumentException("max must be greater than or equal to min.", nameof(max));

            return min + random.Value01() * (max - min);
        }

        /// <summary>An int in [0, count). Handy for picking from a list.</summary>
        public static int Index(this IRandomSource random, int count)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), "count must be positive.");

            // Value01 is < 1, so the result is always < count. Min() guards float rounding.
            return Math.Min(count - 1, (int)(random.Value01() * count));
        }
    }
}
