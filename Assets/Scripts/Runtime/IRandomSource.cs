namespace TestingLab
{
    /// <summary>
    /// Abstraction over randomness so tests can make "random" outcomes deterministic.
    /// </summary>
    public interface IRandomSource
    {
        /// <summary>A value in [0, 1).</summary>
        float Value01();
    }

    public class UnityRandomSource : IRandomSource
    {
        public float Value01()
        {
            // UnityEngine.Random.value can return exactly 1.0, so clamp it below 1.
            float v = UnityEngine.Random.value;
            return v >= 1f ? 0.99999994f : v;
        }
    }
}
