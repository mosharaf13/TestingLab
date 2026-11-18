using TestingLab.Core.Progression;

namespace TestingLab.Gameplay.Infrastructure
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Pattern:        Composition Root
    //
    // The ONE place where the game picks real implementations for its interfaces.
    // Every scene controller asks here instead of calling "new PlayerPrefsStorage()"
    // itself, so swapping storage (e.g. to cloud saves) is a one-line change.
    //
    // Why not a Singleton or a static "Services.Get<T>()" locator? Those hide
    // dependencies: you can't see from a constructor what a class needs, and tests
    // must remember to reset global state. Here, dependencies are visible.
    // ─────────────────────────────────────────────────────────────────────────

    public static class GameServices
    {
        public static IProfileStore CreateProfileStore() => new ProfileRepository(new PlayerPrefsStorage());

        public static RunProgression CreateProgression(int levelCount) =>
            new RunProgression(CreateProfileStore(), new SceneNavigator(), levelCount);
    }
}
