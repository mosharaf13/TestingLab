namespace TestingLab.Core.Progression
{
    // Pattern: Repository. Hides HOW profiles are stored (PlayerPrefs, a file, a
    // server...). Tests use FakeProfileStore, an in-memory version.

    public interface IProfileStore
    {
        /// <returns>The saved profile, or null if there is none.</returns>
        PlayerProfile Load();

        void Save(PlayerProfile profile);

        void Delete();
    }
}
