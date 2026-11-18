using TestingLab.Core.Common;

namespace TestingLab.Tests.Shared.Fakes
{
    // ─────────────────────────────────────────────────────────────────────────
    // A FAKE: a working but simplified implementation for tests.
    // This clock only moves when the test says so — "wait 2 seconds" takes 0 ms.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class FakeTimeProvider : ITimeProvider
    {
        public float Now { get; set; }

        public void Advance(float seconds) => Now += seconds;
    }
}
