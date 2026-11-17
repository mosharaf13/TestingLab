namespace TestingLab
{
    /// <summary>
    /// Abstraction over "what time is it?" so tests can control time
    /// instead of waiting for real seconds to pass.
    /// </summary>
    public interface ITimeProvider
    {
        float Now { get; }
    }

    public class UnityTimeProvider : ITimeProvider
    {
        public float Now => UnityEngine.Time.time;
    }
}
