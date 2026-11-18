using System.Linq;
using TestingLab.Gameplay.Infrastructure;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TestingLab.EditorTools.Builders
{
    // ─────────────────────────────────────────────────────────────────────────
    // Menu: Testing Lab ▸ Rebuild Game Content
    //
    // Regenerates every scene, prefab, material and data asset from code. The
    // generated assets are committed to the project, so you only need this if you
    // break a scene and want a clean copy (it overwrites your edits!).
    //
    // Why build scenes from code in a teaching project? It makes the whole setup
    // reviewable as C#, and it's a nice example of Editor scripting.
    // ─────────────────────────────────────────────────────────────────────────

    public static class GameContentBuilder
    {
        private const string MenuRoot = "Testing Lab/";

        [MenuItem(MenuRoot + "Rebuild Game Content", priority = 0)]
        public static void RebuildFromMenu()
        {
            bool confirmed = EditorUtility.DisplayDialog("Rebuild game content",
                "This regenerates every scene, prefab and data asset in Assets/Scenes, Assets/Prefabs and Assets/Data. " +
                "Any manual changes to them will be lost.", "Rebuild", "Cancel");
            if (confirmed && EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                BuildAll();
        }

        /// <summary>Builds everything without asking. Also callable from the command line (-executeMethod).</summary>
        public static void BuildAll()
        {
            DataAssetBuilder.BuildLevels();
            DataAssetBuilder.BuildShop();
            PrefabBuilder.BuildCoin();
            PrefabBuilder.BuildEnemy();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Each scene builder loads the assets it needs AFTER creating its scene.
            // Creating a scene unloads unused assets, so a ScriptableObject loaded
            // earlier would turn into a dead reference and be saved as "None".
            // (SceneSetupValidationTests caught exactly this bug while this builder
            // was being written. Content tests earn their keep!)
            MainMenuSceneBuilder.Build();
            ArenaSceneBuilder.Build();
            ShopSceneBuilder.Build();

            EditorBuildSettings.scenes = SceneNames.All
                .Select(name => new EditorBuildSettingsScene(ContentPaths.ScenePath(name), true))
                .ToArray();

            AssetDatabase.SaveAssets();
            EditorSceneManager.OpenScene(ContentPaths.ScenePath(SceneNames.MainMenu));
            Debug.Log("Testing Lab: game content rebuilt. Open Assets/Scenes/MainMenu and press Play.");
        }

        [MenuItem(MenuRoot + "Open Main Menu Scene", priority = 20)]
        public static void OpenMainMenu() => OpenScene(SceneNames.MainMenu);

        [MenuItem(MenuRoot + "Open Arena Scene", priority = 21)]
        public static void OpenArena() => OpenScene(SceneNames.Arena);

        [MenuItem(MenuRoot + "Open Shop Scene", priority = 22)]
        public static void OpenShop() => OpenScene(SceneNames.Shop);

        [MenuItem(MenuRoot + "Delete Save Data", priority = 40)]
        public static void DeleteSaveData()
        {
            new PlayerPrefsStorage().DeleteKey(ProfileRepository.DefaultKey);
            Debug.Log("Testing Lab: save data deleted. The next run starts fresh.");
        }

        private static void OpenScene(string sceneName)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(ContentPaths.ScenePath(sceneName));
        }
    }
}
