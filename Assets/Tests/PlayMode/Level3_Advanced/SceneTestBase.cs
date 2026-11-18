using System.Collections;
using NUnit.Framework;
using TestingLab.Core.Progression;
using TestingLab.Gameplay.Infrastructure;
using TestingLab.Tests.Shared.Support;
using UnityEngine.TestTools;

namespace TestingLab.Tests.PlayMode.Level3_Advanced
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED
    // Base class for tests that load REAL game scenes.
    //
    //   • Backs up and restores your save data (PlayerPrefsSandbox).
    //   • Unloads the scene afterwards, so a running Arena can't leak into the
    //     next test. Every test starts from a known, clean state.
    //   • [UnityTearDown] is the coroutine version of [TearDown]: it can wait.
    // ─────────────────────────────────────────────────────────────────────────

    public abstract class SceneTestBase
    {
        private PlayerPrefsSandbox _sandbox;

        protected IProfileStore SaveData { get; private set; }

        [SetUp]
        public void BackUpSaveData()
        {
            _sandbox = new PlayerPrefsSandbox(ProfileRepository.DefaultKey);
            SaveData = GameServices.CreateProfileStore();
            SaveData.Delete(); // start every test with no save
        }

        [UnityTearDown]
        public IEnumerator UnloadAndRestore()
        {
            yield return SceneTestHelpers.UnloadAllScenes();
            _sandbox.Restore();
        }
    }
}
