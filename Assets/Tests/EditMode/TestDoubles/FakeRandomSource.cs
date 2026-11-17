namespace TestingLab.Tests.EditMode
{
    /// <summary>"Random" numbers the test chooses, so outcomes are repeatable.</summary>
    public class FakeRandomSource : IRandomSource
    {
        public float NextValue { get; set; }

        public float Value01() => NextValue;
    }
}
