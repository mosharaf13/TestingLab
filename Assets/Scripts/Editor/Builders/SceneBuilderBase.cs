using TestingLab.Gameplay.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TestingLab.EditorTools.Builders
{
    /// <summary>Shared helpers for the three scene builders.</summary>
    internal static class SceneBuilderBase
    {
        public static Scene NewScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            var camera = Camera.main;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.06f, 0.07f, 0.11f);
            return scene;
        }

        /// <summary>Loads an asset that must exist, or fails loudly.</summary>
        public static T Load<T>(string path) where T : Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
                throw new System.InvalidOperationException($"Expected an asset at {path}.");
            return asset;
        }

        public static void AddLabNotes(string title, string notes)
        {
            var overlay = new GameObject("Lab Notes (F1)").AddComponent<LabNotesOverlay>();
            overlay.Title = title;
            overlay.Notes = notes;
        }

        /// <summary>A centred, vertically stacked box (menus, dialogs).</summary>
        public static RectTransform CreateDialog(Transform parent, string name, Vector2 size)
        {
            var box = UiFactory.CreatePanel(parent, name, UiTheme.Panel);
            UiFactory.Place(box.rectTransform, new Vector2(0.5f, 0.5f), size);
            UiFactory.AddVerticalLayout(box.gameObject, spacing: 18f, padding: 36);
            // Height follows the content, so dialogs never have an empty bottom half.
            box.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            return box.rectTransform;
        }

        /// <summary>A full-screen dimmed overlay, initially hidden.</summary>
        public static GameObject CreateOverlay(Transform parent, string name)
        {
            var dim = UiFactory.CreatePanel(parent, name, new Color(0f, 0f, 0f, 0.65f));
            UiFactory.Stretch(dim.rectTransform);
            return dim.gameObject;
        }

        public static Text Heading(Transform parent, string name, string text, int size = UiTheme.HeadingSize)
        {
            var label = UiFactory.CreateText(parent, name, text, size);
            UiFactory.SetPreferredSize(label, height: size * 1.6f);
            return label;
        }

        public static void Save(Scene scene, string sceneName)
        {
            System.IO.Directory.CreateDirectory(ContentPaths.ScenesFolder);
            EditorSceneManager.SaveScene(scene, ContentPaths.ScenePath(sceneName));
        }
    }
}
