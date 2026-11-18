using TestingLab.Core.Progression;

namespace TestingLab.Tests.Shared.Fakes
{
    /// <summary>
    /// FAKE + SPY: stores the profile in memory AND records how it was used,
    /// so tests can ask "was Save called?" and "what was saved?".
    /// </summary>
    public sealed class FakeProfileStore : IProfileStore
    {
        public PlayerProfile Stored { get; set; }
        public int SaveCount { get; private set; }
        public int DeleteCount { get; private set; }

        public FakeProfileStore(PlayerProfile initial = null) => Stored = initial;

        public PlayerProfile Load() => Stored;

        public void Save(PlayerProfile profile)
        {
            SaveCount++;
            Stored = profile;
        }

        public void Delete()
        {
            DeleteCount++;
            Stored = null;
        }
    }
}
