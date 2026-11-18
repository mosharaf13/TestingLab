namespace TestingLab.Gameplay.Infrastructure
{
    /// <summary>
    /// One place for scene names. A typo in a string is a runtime bug; a typo in a
    /// constant is a compile error. SceneSetupValidationTests checks these scenes
    /// really exist in Build Settings.
    /// </summary>
    public static class SceneNames
    {
        public const string MainMenu = "MainMenu";
        public const string Arena = "Arena";
        public const string Shop = "Shop";

        public static readonly string[] All = { MainMenu, Arena, Shop };
    }
}
