namespace TestingLab.Core.Common
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE  (test doubles / dependency inversion)
    //
    // Randomness makes tests flaky: a test that passes 90% of the time is worse
    // than no test. Code that needs a dice roll asks this interface, so a test can
    // hand it a FakeRandomSource that returns exactly the numbers it wants.
    //
    // Implementations: SystemRandomSource (seeded, pure C#)
    //                  Gameplay/Infrastructure/UnityRandomSource.cs
    // Test double:     Tests/Shared/Fakes/FakeRandomSource.cs
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>Abstraction over randomness so "random" outcomes can be made deterministic.</summary>
    public interface IRandomSource
    {
        /// <summary>A value in [0, 1). Never returns exactly 1.</summary>
        float Value01();
    }
}
