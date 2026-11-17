namespace TestingLab.Tests.EditMode
{
    /// <summary>A clock the test controls. No waiting for real time.</summary>
    public class FakeTimeProvider : ITimeProvider
    {
        public float Now { get; set; }

        public void Advance(float seconds) => Now += seconds;
    }
}
