using System.Collections.Generic;
using System.IO;
using TestingLab.Gameplay.Config;
using UnityEditor;
using UnityEngine;

namespace TestingLab.EditorTools.Builders
{
    /// <summary>
    /// Creates the level and shop ScriptableObjects. After the first build these are
    /// ordinary assets: designers (and students) tweak them in the Inspector.
    /// Rebuilding overwrites them with these defaults.
    /// </summary>
    internal static class DataAssetBuilder
    {
        public static LevelCatalog BuildLevels()
        {
            var levels = new List<LevelConfig>
            {
                Level("Level1_WarmUp", "Warm-up", time: 60f, goal: 10, points: 10, speed: 2.0f, damage: 10,
                    new WaveEntry(0f, 5, 0), new WaveEntry(6f, 4, 1), new WaveEntry(14f, 4, 1), new WaveEntry(24f, 3, 2)),
                Level("Level2_Crowded", "Crowded", time: 60f, goal: 14, points: 15, speed: 2.6f, damage: 15,
                    new WaveEntry(0f, 4, 1), new WaveEntry(5f, 4, 1), new WaveEntry(12f, 4, 2), new WaveEntry(20f, 4, 2), new WaveEntry(30f, 3, 2)),
                Level("Level3_Gauntlet", "Gauntlet", time: 75f, goal: 18, points: 20, speed: 3.2f, damage: 20,
                    new WaveEntry(0f, 4, 2), new WaveEntry(6f, 4, 2), new WaveEntry(14f, 4, 3), new WaveEntry(24f, 4, 3), new WaveEntry(36f, 4, 4)),
            };

            var catalog = GetOrCreate<LevelCatalog>(ContentPaths.LevelCatalog);
            catalog.Configure(levels);
            EditorUtility.SetDirty(catalog);
            return catalog;
        }

        public static ShopCatalog BuildShop()
        {
            var catalog = GetOrCreate<ShopCatalog>(ContentPaths.ShopCatalog);
            catalog.Configure(new[]
            {
                new ShopItemDefinition("heart", "Heart Container", 8, ItemEffectKind.MaxHealth, 25f),
                new ShopItemDefinition("armor", "Armor Plate", 10, ItemEffectKind.Armor, 20f),
                new ShopItemDefinition("boots", "Swift Boots", 6, ItemEffectKind.MoveSpeed, 1f),
            });
            EditorUtility.SetDirty(catalog);
            return catalog;
        }

        private static LevelConfig Level(string assetName, string displayName, float time, int goal, int points,
            float speed, int damage, params WaveEntry[] waves)
        {
            var level = GetOrCreate<LevelConfig>($"{ContentPaths.LevelsFolder}/{assetName}.asset");
            level.Configure(displayName, time, goal, points, speed, damage, waves);
            EditorUtility.SetDirty(level);
            return level;
        }

        private static T GetOrCreate<T>(string path) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null)
                return asset;

            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }
    }
}
