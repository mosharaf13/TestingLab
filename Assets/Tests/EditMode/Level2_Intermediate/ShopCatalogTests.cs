using NUnit.Framework;
using TestingLab.Core.Progression;
using TestingLab.Gameplay.Config;
using TestingLab.Tests.Shared;
using UnityEngine;

namespace TestingLab.Tests.EditMode.Level2_Intermediate
{
    // Learning level: INTERMEDIATE
    // Topic: ScriptableObjects in tests. CreateInstance makes a throw-away one in
    // memory (no asset file), so the test controls its exact contents.

    [Category(TestCategory.Intermediate)]
    public class ShopCatalogTests
    {
        private ShopCatalog _catalog;

        [SetUp]
        public void SetUp()
        {
            _catalog = ScriptableObject.CreateInstance<ShopCatalog>();
            _catalog.Configure(new[]
            {
                new ShopItemDefinition("armor", "Armor Plate", 10, ItemEffectKind.Armor, 20f),
                new ShopItemDefinition("boots", "Swift Boots", 6, ItemEffectKind.MoveSpeed, 1f),
            });
        }

        [TearDown]
        public void TearDown() => Object.DestroyImmediate(_catalog);

        [Test]
        public void GetEffect_KnownSku_CreatesTheMatchingEffect()
        {
            var stats = new PlayerStats();

            _catalog.GetEffect("armor").Apply(stats);

            Assert.That(stats.Armor, Is.EqualTo(20));
        }

        [Test]
        public void GetEffect_UnknownSku_ReturnsNull()
        {
            Assert.That(_catalog.GetEffect("nope"), Is.Null);
        }

        [Test]
        public void Find_ReturnsTheDefinition()
        {
            Assert.That(_catalog.Find("boots").Price, Is.EqualTo(6));
        }
    }
}
