namespace TestingLab.Tests.Shared.Builders
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED
    // Pattern:        Test Data Builder (with a tiny "A.Thing()" entry point)
    //
    //   var profile = A.Profile().WithCoins(50).AtLevel(2).Build();
    //
    // Reads like a sentence and only mentions what THIS test cares about.
    // When PlayerProfile's constructor changes, only the builder needs fixing,
    // not 40 tests.
    // ─────────────────────────────────────────────────────────────────────────

    public static class A
    {
        public static PlayerProfileBuilder Profile() => new PlayerProfileBuilder();
    }
}
