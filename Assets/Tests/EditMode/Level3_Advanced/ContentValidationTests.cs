using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestingLab.EditorTools.Builders;
using TestingLab.Gameplay.Config;
using TestingLab.Tests.Shared;
using UnityEditor;

namespace TestingLab.Tests.EditMode.Level3_Advanced
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED
    // Topic: CONTENT / DATA tests. These test ASSETS, not code.
    //
    // Designers change numbers in the Inspector all the time. These tests find
    // every LevelConfig and the ShopCatalog in the project and check them, so a
    // bad edit fails the build instead of reaching players.
    //
    // [TestCaseSource] calls AssetDatabase to build the list, so the Test Runner
    // shows one test PER LEVEL ASSET. Add Level4.asset and a new test appears.
    // Try it: set Level1's coin goal to 99 and run these tests.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Advanced)]
    public class ContentValidationTests
    {
        private static IEnumerable<TestCaseData> AllLevelAssets()
        {
            foreach (string guid in AssetDatabase.FindAssets("t:" + nameof(LevelConfig)))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                yield return new TestCaseData(path).SetName($"Level asset is valid: {System.IO.Path.GetFileNameWithoutExtension(path)}");
            }
        }

        [TestCaseSource(nameof(AllLevelAssets))]
        public void LevelAsset_PassesLevelRules(string assetPath)
        {
            var level = AssetDatabase.LoadAssetAtPath<LevelConfig>(assetPath);

            var errors = level.Validate();

            // Join all problems into one message so you see every issue at once.
            Assert.That(errors, Is.Empty, $"{assetPath}:\n  " + string.Join("\n  ", errors));
        }

        [Test]
        public void LevelCatalog_HasLevels_AndNoEmptySlots()
        {
            var catalog = LoadOrIgnore<LevelCatalog>(ContentPaths.LevelCatalog);

            Assert.That(catalog.Count, Is.GreaterThan(0));
            Assert.That(catalog.Levels, Has.None.Null, "an empty slot in the list would crash the Arena");
        }

        [Test]
        public void LevelCatalog_GetsHarderEachLevel()
        {
            // A design rule written as a test. Debatable? Good: discuss it in class.
            var levels = LoadOrIgnore<LevelCatalog>(ContentPaths.LevelCatalog).Levels;

            for (int i = 1; i < levels.Count; i++)
                Assert.That(levels[i].EnemySpeed, Is.GreaterThanOrEqualTo(levels[i - 1].EnemySpeed),
                    $"{levels[i].name} should not have slower enemies than {levels[i - 1].name}");
        }

        [Test]
        public void ShopCatalog_SkusAreUnique()
        {
            var catalog = LoadOrIgnore<ShopCatalog>(ContentPaths.ShopCatalog);
            var duplicates = catalog.Items.GroupBy(i => i.Sku).Where(g => g.Count() > 1).Select(g => g.Key).ToList();

            Assert.That(duplicates, Is.Empty, "duplicate SKUs: " + string.Join(", ", duplicates));
        }

        [Test]
        public void ShopCatalog_EveryItemHasANamePriceAndWorkingEffect()
        {
            var catalog = LoadOrIgnore<ShopCatalog>(ContentPaths.ShopCatalog);

            // Collect EVERY problem, then assert once, so one run shows all of them.
            // (Unity's NUnit version has no Assert.Multiple, so we do it by hand.)
            var problems = new List<string>();
            foreach (var item in catalog.Items)
            {
                if (string.IsNullOrEmpty(item.Sku)) problems.Add("an item has no SKU");
                if (string.IsNullOrEmpty(item.DisplayName)) problems.Add($"{item.Sku} has no display name");
                if (item.Price <= 0) problems.Add($"{item.Sku} is free");
                try { item.CreateEffect(); }
                catch (System.Exception e) { problems.Add($"{item.Sku} has a broken effect: {e.Message}"); }
            }

            Assert.That(problems, Is.Empty, string.Join("\n", problems));
        }

        private static T LoadOrIgnore<T>(string path) where T : UnityEngine.Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
                Assert.Ignore($"{path} not found. Run Testing Lab ▸ Rebuild Game Content.");
            return asset;
        }
    }
}
