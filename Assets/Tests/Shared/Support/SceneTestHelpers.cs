using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TestingLab.Tests.Shared.Support
{
    /// <summary>Loading real game scenes in Play Mode tests, and cleaning up afterwards.</summary>
    public static class SceneTestHelpers
    {
        public static IEnumerator LoadScene(string sceneName, float timeoutSeconds = 10f)
        {
            SceneManager.LoadScene(sceneName);
            yield return WaitFor.Condition(() => SceneManager.GetActiveScene().name == sceneName,
                timeoutSeconds, $"scene '{sceneName}' never became active (is it in Build Settings?)");
            yield return null; // let Start() run on everything in the new scene
        }

        /// <summary>
        /// Swaps every loaded scene for an empty one, so a test can't leave a running
        /// Arena behind that interferes with the next test.
        /// </summary>
        public static IEnumerator UnloadAllScenes()
        {
            Time.timeScale = 1f;
            var empty = SceneManager.CreateScene("Empty Test Scene " + Time.frameCount);
            SceneManager.SetActiveScene(empty);

            var toUnload = new List<Scene>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene != empty)
                    toUnload.Add(scene);
            }

            foreach (var scene in toUnload)
                yield return SceneManager.UnloadSceneAsync(scene);
        }
    }
}
