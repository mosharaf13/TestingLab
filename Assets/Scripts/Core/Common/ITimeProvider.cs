namespace TestingLab.Core.Common
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE  (test doubles / dependency inversion)
    // Pattern:        Dependency Inversion Principle (the "D" in SOLID)
    //
    // Game code asks this interface "what time is it?" instead of reading
    // UnityEngine.Time directly. Real games pass UnityTimeProvider; tests pass
    // FakeTimeProvider and move time forward instantly with Advance(seconds).
    //
    // Implementations: Gameplay/Infrastructure/UnityTimeProvider.cs
    // Test double:     Tests/Shared/Fakes/FakeTimeProvider.cs
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>Abstraction over "what time is it?" so tests can control time.</summary>
    public interface ITimeProvider
    {
        /// <summary>Seconds since the game started.</summary>
        float Now { get; }
    }
}
