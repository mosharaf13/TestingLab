using System;
using TestingLab.Core.Common;
using TestingLab.Core.Progression;
using UnityEngine;

namespace TestingLab.Gameplay.Infrastructure
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: INTERMEDIATE
    // Tested by:      Tests/EditMode/Level2_Intermediate/ProfileRepositoryTests.cs
    // Pattern:        Repository (implements Core's IProfileStore)
    //
    // Saves the profile as JSON into an IKeyValueStorage. Because the storage is
    // injected, tests use an in-memory dictionary and never touch real save data.
    // A corrupted save is treated as "no save" instead of crashing the game.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class ProfileRepository : IProfileStore
    {
        public const string DefaultKey = "TestingLab.Profile";

        private readonly IKeyValueStorage _storage;
        private readonly string _key;

        public ProfileRepository(IKeyValueStorage storage, string key = DefaultKey)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("A storage key is required.", nameof(key));
            _key = key;
        }

        public PlayerProfile Load()
        {
            string json = _storage.GetString(_key);
            if (string.IsNullOrEmpty(json))
                return null;

            try
            {
                var dto = JsonUtility.FromJson<ProfileDto>(json);
                return dto?.ToProfile();
            }
            catch (ArgumentException e)
            {
                Debug.LogWarning($"Ignoring unreadable save data: {e.Message}");
                return null;
            }
        }

        public void Save(PlayerProfile profile)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            _storage.SetString(_key, JsonUtility.ToJson(ProfileDto.FromProfile(profile)));
        }

        public void Delete() => _storage.DeleteKey(_key);
    }
}
