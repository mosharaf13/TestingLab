namespace TestingLab.Core.Common
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED  (contract tests)
    // Pattern:        Adapter / Dependency Inversion
    //
    // A minimal "save slot" abstraction. The game uses PlayerPrefsStorage; tests use
    // InMemoryKeyValueStorage so they never touch the real save file.
    // Both implementations are checked by the SAME abstract test class
    // (Tests/EditMode/Level3_Advanced/Contracts/KeyValueStorageContract.cs).
    // ─────────────────────────────────────────────────────────────────────────

    public interface IKeyValueStorage
    {
        bool HasKey(string key);

        /// <returns>The stored value, or null if the key does not exist.</returns>
        string GetString(string key);

        void SetString(string key, string value);

        void DeleteKey(string key);
    }
}
