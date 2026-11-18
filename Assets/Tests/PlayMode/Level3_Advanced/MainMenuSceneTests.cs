using System.Collections;
using NUnit.Framework;
using TestingLab.Gameplay.Flow;
using TestingLab.Gameplay.Infrastructure;
using TestingLab.Gameplay.UI;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Builders;
using TestingLab.Tests.Shared.Support;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace TestingLab.Tests.PlayMode.Level3_Advanced
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED
    // Scene under test: Assets/Scenes/MainMenu.unity
    //
    // Topic: UI / INTEGRATION tests. Load the real scene, click real buttons,
    // check the real result (another scene loads). Each test covers a whole
    // chain: Button → View event → Controller → RunProgression → SceneNavigator.
    //
    // These are the slowest, most fragile kind of test, so we write FEW of them:
    // just enough to prove the pieces are connected. The logic underneath is
    // already tested in Edit Mode (RunProgressionTests).
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Advanced), Category(TestCategory.Integration)]
    [Timeout(60000)] // safety net: a stuck scene test fails after 60 s instead of hanging the runner
    public class MainMenuSceneTests : SceneTestBase
    {
        [UnityTest]
        public IEnumerator FirstLaunch_ShowsZeroBestScore_AndHidesContinue()
        {
            yield return SceneTestHelpers.LoadScene(SceneNames.MainMenu);

            var view = Object.FindFirstObjectByType<MainMenuView>();
            Assert.That(view.BestScoreText, Is.EqualTo("Best score: 0"));
            Assert.That(view.IsContinueVisible, Is.False, "nothing to continue on a first launch");
        }

        [UnityTest]
        public IEnumerator ClickingNewRun_LoadsTheArena()
        {
            yield return SceneTestHelpers.LoadScene(SceneNames.MainMenu);

            UiTestHelpers.Click("PlayButton");

            yield return WaitFor.Condition(() => SceneManager.GetActiveScene().name == SceneNames.Arena, 5f, "Arena never loaded");
            yield return null;
            Assert.That(Object.FindFirstObjectByType<ArenaController>(), Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator SavedRun_ShowsBestScore_AndContinueGoesToShop()
        {
            SaveData.Save(A.Profile().AtLevel(1).WithBestScore(500));

            yield return SceneTestHelpers.LoadScene(SceneNames.MainMenu);
            var view = Object.FindFirstObjectByType<MainMenuView>();
            Assert.That(view.BestScoreText, Is.EqualTo("Best score: 500"));
            Assert.That(view.IsContinueVisible, Is.True);

            UiTestHelpers.Click("ContinueButton");

            yield return WaitFor.Condition(() => SceneManager.GetActiveScene().name == SceneNames.Shop, 5f, "Shop never loaded");
        }
    }
}
