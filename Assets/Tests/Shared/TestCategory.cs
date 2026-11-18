namespace TestingLab.Tests.Shared
{
    /// <summary>
    /// Category names for [Category(...)]. In the Test Runner window, use the
    /// category dropdown to run only "Beginner" tests, only "Advanced", etc.
    /// Constants (not typed strings) so a typo is a compile error.
    /// </summary>
    public static class TestCategory
    {
        public const string Beginner = "Beginner";
        public const string Intermediate = "Intermediate";
        public const string Advanced = "Advanced";

        /// <summary>Student tasks. Ignored until a student starts them.</summary>
        public const string Exercise = "Exercise";

        /// <summary>Loads real scenes. Slower; run them less often while coding.</summary>
        public const string Integration = "Integration";
    }
}
