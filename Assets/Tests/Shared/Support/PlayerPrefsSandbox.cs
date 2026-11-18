using UnityEngine;

namespace TestingLab.Tests.Shared.Support
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED
    //
    // Scene tests run the REAL game, which saves to PlayerPrefs. Without care, a
    // test would wipe the save you made while playing (and leftover saves from
    // one test would change the result of the next: tests must be independent).
    //
    // The sandbox remembers the original value in [SetUp] and puts it back in
    // [TearDown], whatever the test did in between.
    // ─────────────────────────────────────────────────────────────────────────

    public sealed class PlayerPrefsSandbox
    {
        private readonly string _key;
        private readonly bool _hadValue;
        private readonly string _originalValue;

        public PlayerPrefsSandbox(string key)
        {
            _key = key;
            _hadValue = PlayerPrefs.HasKey(key);
            _originalValue = _hadValue ? PlayerPrefs.GetString(key) : null;
        }

        public void Restore()
        {
            if (_hadValue)
                PlayerPrefs.SetString(_key, _originalValue);
            else
                PlayerPrefs.DeleteKey(_key);
            PlayerPrefs.Save();
        }
    }
}
