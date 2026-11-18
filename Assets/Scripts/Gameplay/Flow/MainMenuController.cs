using TestingLab.Core.Progression;
using TestingLab.Gameplay.Config;
using TestingLab.Gameplay.Infrastructure;
using TestingLab.Gameplay.UI;
using UnityEngine;

namespace TestingLab.Gameplay.Flow
{
    /// <summary>
    /// Presenter for the main menu: reads the saved profile, fills in the view and
    /// turns button clicks into RunProgression calls.
    /// </summary>
    public sealed class MainMenuController : MonoBehaviour
    {
        [SerializeField] private LevelCatalog levels;
        [SerializeField] private MainMenuView view;

        public RunProgression Progression { get; private set; }

        private void Start()
        {
            Time.timeScale = 1f;
            Progression = GameServices.CreateProgression(levels.Count);
            var profile = Progression.Profile;

            view.SetBestScore(profile.BestScore);
            view.SetContinue(Progression.HasRunInProgress, $"Continue (Level {profile.LevelIndex + 1})");
            view.SetMessage(Progression.IsRunComplete
                ? "You cleared every level! Start a new run to beat your best score."
                : "WASD to move · collect coins · avoid red enemies · Esc to pause");

            view.PlayClicked += Progression.StartNewRun;
            view.ContinueClicked += Progression.ContinueRun;
            view.QuitClicked += Quit;
        }

        private static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
