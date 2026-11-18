using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestingLab.EditorTools.Builders;
using TestingLab.Gameplay.Infrastructure;
using TestingLab.Tests.Shared;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TestingLab.Tests.EditMode.Level3_Advanced
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED
    // Topic: SCENE / PREFAB VALIDATION ("smoke tests" for content).
    //
    // The most common Unity bug isn't wrong maths. It's "I forgot to drag the
    // reference into the Inspector" → NullReferenceException at runtime.
    // These tests open every scene WITHOUT pressing Play and check:
    //   • the scene is in Build Settings (else LoadScene fails in a build)
    //   • no "Missing Script" components
    //   • every [SerializeField] reference on OUR components is assigned
    //   • a Camera exists, and an EventSystem exists if there are buttons
    // They're fast (no Play mode) and catch whole categories of mistakes.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Advanced)]
    public class SceneSetupValidationTests
    {
        private const string OurNamespace = "TestingLab";

        private static IEnumerable<string> GameScenes() => SceneNames.All;

        private Scene _scene;
        private bool _openedByTest;

        [TearDown]
        public void CloseScene()
        {
            if (_openedByTest && _scene.IsValid())
                EditorSceneManager.CloseScene(_scene, removeScene: true);
            _openedByTest = false;
        }

        [TestCaseSource(nameof(GameScenes))]
        public void Scene_IsEnabledInBuildSettings(string sceneName)
        {
            string path = ContentPaths.ScenePath(sceneName);

            Assert.That(EditorBuildSettings.scenes.Any(s => s.path == path && s.enabled), Is.True,
                $"{path} is missing from File ▸ Build Profiles ▸ Scene List");
        }

        [TestCaseSource(nameof(GameScenes))]
        public void Scene_HasNoMissingScripts(string sceneName)
        {
            OpenAdditively(sceneName);

            var broken = AllGameObjects()
                .Where(go => GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go) > 0)
                .Select(PathOf)
                .ToList();

            Assert.That(broken, Is.Empty, "Objects with missing scripts:\n  " + string.Join("\n  ", broken));
        }

        [TestCaseSource(nameof(GameScenes))]
        public void Scene_AllSerializedReferencesAreAssigned(string sceneName)
        {
            OpenAdditively(sceneName);

            var unassigned = new List<string>();
            foreach (var component in AllGameObjects().SelectMany(go => go.GetComponents<MonoBehaviour>()))
            {
                if (component == null || component.GetType().Namespace?.StartsWith(OurNamespace) != true)
                    continue;

                var property = new SerializedObject(component).GetIterator();
                while (property.NextVisible(enterChildren: true))
                {
                    if (property.propertyType == SerializedPropertyType.ObjectReference
                        && property.objectReferenceValue == null
                        && property.name != "m_Script")
                    {
                        unassigned.Add($"{PathOf(component.gameObject)} ▸ {component.GetType().Name}.{property.name}");
                    }
                }
            }

            Assert.That(unassigned, Is.Empty, "Unassigned references:\n  " + string.Join("\n  ", unassigned));
        }

        [TestCaseSource(nameof(GameScenes))]
        public void Scene_HasCamera_AndEventSystemWhenItHasButtons(string sceneName)
        {
            OpenAdditively(sceneName);
            var objects = AllGameObjects().ToList();

            Assert.That(objects.Count(go => go.GetComponent<Camera>() != null), Is.EqualTo(1), "exactly one camera");

            bool hasButtons = objects.Any(go => go.GetComponent<Selectable>() != null);
            int eventSystems = objects.Count(go => go.GetComponent<EventSystem>() != null);
            if (hasButtons)
                Assert.That(eventSystems, Is.EqualTo(1), "buttons need exactly one EventSystem, or clicks do nothing");
        }

        [Test]
        public void CoinPrefab_IsATriggerCollectible()
        {
            var coin = LoadPrefabOrIgnore(ContentPaths.CoinPrefab);

            Assert.That(coin.GetComponent<TestingLab.Gameplay.Pickups.Collectible>(), Is.Not.Null);
            Assert.That(coin.GetComponent<Collider>().isTrigger, Is.True, "pickups must be triggers");
        }

        [Test]
        public void EnemyPrefab_CanMoveAndHurt()
        {
            var enemy = LoadPrefabOrIgnore(ContentPaths.EnemyPrefab);

            Assert.That(enemy.GetComponent<TestingLab.Gameplay.Enemies.EnemyController>(), Is.Not.Null);
            Assert.That(enemy.GetComponent<TestingLab.Gameplay.Combat.ContactDamage>(), Is.Not.Null);
            Assert.That(enemy.GetComponent<Collider>().isTrigger, Is.True, "contact damage uses OnTriggerStay");
            Assert.That(enemy.GetComponent<Rigidbody>().isKinematic, Is.True, "moved by code, not by physics");
        }

        // ── helpers ────────────────────────────────────────────────────────────

        private void OpenAdditively(string sceneName)
        {
            string path = ContentPaths.ScenePath(sceneName);
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(path) == null)
                Assert.Ignore($"{path} not found. Run Testing Lab ▸ Rebuild Game Content.");

            _scene = SceneManager.GetSceneByPath(path);
            if (_scene.IsValid() && _scene.isLoaded)
                return; // already open in the editor: inspect it, but don't close it afterwards

            _scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
            _openedByTest = true;
        }

        private IEnumerable<GameObject> AllGameObjects() =>
            _scene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<Transform>(includeInactive: true))
                .Select(t => t.gameObject);

        private static string PathOf(GameObject go)
        {
            string path = go.name;
            for (var parent = go.transform.parent; parent != null; parent = parent.parent)
                path = parent.name + "/" + path;
            return path;
        }

        private static GameObject LoadPrefabOrIgnore(string path)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
                Assert.Ignore($"{path} not found. Run Testing Lab ▸ Rebuild Game Content.");
            return prefab;
        }
    }
}
