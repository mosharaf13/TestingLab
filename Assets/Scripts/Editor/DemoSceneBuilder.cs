using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TestingLab.EditorTools
{
    /// <summary>
    /// Menu: Testing Lab > Build Demo Scene.
    /// Creates a small playable scene (player + coins) at Assets/Scenes/Demo.unity.
    /// </summary>
    public static class DemoSceneBuilder
    {
        private const string ScenePath = "Assets/Scenes/Demo.unity";
        private const string CoinMaterialPath = "Assets/Materials/Coin.mat";

        [MenuItem("Testing Lab/Build Demo Scene")]
        public static void Build()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(2f, 1f, 2f);

            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.transform.position = new Vector3(0f, 1f, 0f);
            var body = player.AddComponent<Rigidbody>();
            body.isKinematic = true;
            player.AddComponent<Collector>();
            player.AddComponent<HealthComponent>();
            player.AddComponent<DemoPlayerInput>();

            var coinMaterial = GetOrCreateCoinMaterial();
            const int coinCount = 8;
            for (int i = 0; i < coinCount; i++)
            {
                float angle = i * Mathf.PI * 2f / coinCount;
                var coin = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                coin.name = $"Coin {i + 1}";
                coin.transform.position = new Vector3(Mathf.Cos(angle) * 6f, 1f, Mathf.Sin(angle) * 6f);
                coin.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                coin.transform.localScale = new Vector3(0.8f, 0.05f, 0.8f);
                coin.GetComponent<Collider>().isTrigger = true;
                coin.GetComponent<Renderer>().sharedMaterial = coinMaterial;
                coin.AddComponent<Collectible>();
                coin.AddComponent<Spinner>();
            }

            var camera = Camera.main;
            if (camera != null)
            {
                camera.transform.position = new Vector3(0f, 14f, -12f);
                camera.transform.LookAt(Vector3.zero);
            }

            System.IO.Directory.CreateDirectory("Assets/Scenes");
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            Debug.Log("Demo scene built at " + ScenePath + ". Press Play and use WASD to collect coins.");
        }

        private static Material GetOrCreateCoinMaterial()
        {
            var existing = AssetDatabase.LoadAssetAtPath<Material>(CoinMaterialPath);
            if (existing != null)
                return existing;

            System.IO.Directory.CreateDirectory("Assets/Materials");
            var shader = Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit");
            var material = new Material(shader) { color = new Color(1f, 0.8f, 0.1f) };
            AssetDatabase.CreateAsset(material, CoinMaterialPath);
            AssetDatabase.SaveAssets();
            return material;
        }
    }
}
