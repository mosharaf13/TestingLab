using System.IO;
using TestingLab.Gameplay.Combat;
using TestingLab.Gameplay.Enemies;
using TestingLab.Gameplay.Pickups;
using UnityEditor;
using UnityEngine;

namespace TestingLab.EditorTools.Builders
{
    /// <summary>Builds the Coin and Enemy prefabs that WaveSpawner instantiates.</summary>
    internal static class PrefabBuilder
    {
        public static GameObject BuildCoin()
        {
            var coin = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            coin.name = "Coin";
            coin.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            coin.transform.localScale = new Vector3(0.8f, 0.05f, 0.8f);
            coin.GetComponent<Renderer>().sharedMaterial = MaterialLibrary.Coin;

            Object.DestroyImmediate(coin.GetComponent<Collider>());
            var trigger = coin.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.radius = 0.6f;

            coin.AddComponent<Collectible>();
            coin.AddComponent<Spinner>();
            return Save(coin, ContentPaths.CoinPrefab);
        }

        public static GameObject BuildEnemy()
        {
            var enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Enemy";
            enemy.GetComponent<Renderer>().sharedMaterial = MaterialLibrary.Enemy;

            var collider = enemy.GetComponent<CapsuleCollider>();
            collider.isTrigger = true;
            collider.radius = 0.6f;

            var body = enemy.AddComponent<Rigidbody>();
            body.isKinematic = true;
            body.useGravity = false;

            AddEyes(enemy.transform);
            enemy.AddComponent<Mover>();
            enemy.AddComponent<EnemyController>();
            enemy.AddComponent<ContactDamage>();
            return Save(enemy, ContentPaths.EnemyPrefab);
        }

        /// <summary>Two little white cubes so you can see which way something faces.</summary>
        public static void AddEyes(Transform parent)
        {
            for (int side = -1; side <= 1; side += 2)
            {
                var eye = GameObject.CreatePrimitive(PrimitiveType.Cube);
                eye.name = side < 0 ? "Eye L" : "Eye R";
                Object.DestroyImmediate(eye.GetComponent<Collider>());
                eye.transform.SetParent(parent, false);
                eye.transform.localPosition = new Vector3(0.18f * side, 0.45f, 0.42f);
                eye.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                eye.GetComponent<Renderer>().sharedMaterial = MaterialLibrary.Accent;
            }
        }

        private static GameObject Save(GameObject instance, string path)
        {
            Directory.CreateDirectory(ContentPaths.PrefabsFolder);
            var prefab = PrefabUtility.SaveAsPrefabAsset(instance, path);
            Object.DestroyImmediate(instance);
            return prefab;
        }
    }
}
