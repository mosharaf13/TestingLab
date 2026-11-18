using TestingLab.Core.Common;
using UnityEngine;

namespace TestingLab.Gameplay.Infrastructure
{
    // Learning level: ADVANCED (checked by KeyValueStorageContract tests)
    // Pattern: Adapter. Makes Unity's static PlayerPrefs API fit our IKeyValueStorage.

    public sealed class PlayerPrefsStorage : IKeyValueStorage
    {
        public bool HasKey(string key) => PlayerPrefs.HasKey(key);

        public string GetString(string key) => PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : null;

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }
    }
}
