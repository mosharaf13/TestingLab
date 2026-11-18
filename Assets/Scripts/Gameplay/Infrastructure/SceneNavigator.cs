using TestingLab.Core.Progression;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TestingLab.Gameplay.Infrastructure
{
    /// <summary>
    /// Humble Object: the only class that actually loads scenes. Everything that
    /// decides WHICH scene to load lives in RunProgression (Core) and is unit tested.
    /// </summary>
    public sealed class SceneNavigator : IGameNavigator
    {
        public void GoToMainMenu() => Load(SceneNames.MainMenu);

        public void GoToArena() => Load(SceneNames.Arena);

        public void GoToShop() => Load(SceneNames.Shop);

        private static void Load(string sceneName)
        {
            Time.timeScale = 1f; // never carry a paused clock into the next scene
            SceneManager.LoadScene(sceneName);
        }
    }
}
