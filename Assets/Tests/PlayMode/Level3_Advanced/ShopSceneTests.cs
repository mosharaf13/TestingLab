using System.Collections;
using NUnit.Framework;
using TestingLab.Gameplay.Flow;
using TestingLab.Gameplay.Infrastructure;
using TestingLab.Gameplay.UI;
using TestingLab.Tests.Shared;
using TestingLab.Tests.Shared.Builders;
using TestingLab.Tests.Shared.Support;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace TestingLab.Tests.PlayMode.Level3_Advanced
{
    // ─────────────────────────────────────────────────────────────────────────
    // Learning level: ADVANCED
    // Scene under test: Assets/Scenes/Shop.unity
    //
    // Topic: a full user journey through the UI, checking the SAVED result.
    // "Arrange" here means writing a save file; "Assert" means reading it back.
    //
    // These tests deliberately avoid the ShopCart bugs (one of each item only).
    // Once students fix the bugs, a good extension is a UI test that buys two.
    // ─────────────────────────────────────────────────────────────────────────

    [Category(TestCategory.Advanced), Category(TestCategory.Integration)]
    [Timeout(60000)] // safety net: a stuck scene test fails after 60 s instead of hanging the runner
    public class ShopSceneTests : SceneTestBase
    {
        private ShopView _view;

        private IEnumerator LoadShop()
        {
            yield return SceneTestHelpers.LoadScene(SceneNames.Shop);
            var controller = Object.FindFirstObjectByType<ShopController>();
            yield return WaitFor.Condition(() => controller.Progression != null, 2f, "ShopController never started");
            _view = Object.FindFirstObjectByType<ShopView>();
        }

        [UnityTest]
        public IEnumerator BuyingArmor_SpendsCoins_AndSavesTheUpgrade()
        {
            SaveData.Save(A.Profile().AtLevel(1).WithCoins(30));
            yield return LoadShop();

            UiTestHelpers.Click("Add_armor");
            Assert.That(_view.CartText, Does.Contain("1 x armor"));

            UiTestHelpers.Click("CheckoutButton");

            Assert.That(_view.MessageText, Does.StartWith("Purchased"));
            var saved = SaveData.Load();
            Assert.That(saved.Wallet.Coins, Is.EqualTo(20));
            Assert.That(saved.Stats.Armor, Is.EqualTo(20));
        }

        [UnityTest]
        public IEnumerator CheckoutWithoutEnoughCoins_ShowsAMessage_AndSavesNothing()
        {
            SaveData.Save(A.Profile().AtLevel(1).WithCoins(3));
            yield return LoadShop();

            UiTestHelpers.Click("Add_armor");
            UiTestHelpers.Click("CheckoutButton");

            Assert.That(_view.MessageText, Is.EqualTo("Not enough coins."));
            Assert.That(SaveData.Load().Wallet.Coins, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator NextLevelButton_LoadsTheArena()
        {
            SaveData.Save(A.Profile().AtLevel(1));
            yield return LoadShop();

            UiTestHelpers.Click("LeaveButton");

            yield return WaitFor.Condition(() => SceneManager.GetActiveScene().name == SceneNames.Arena, 5f, "Arena never loaded");
        }
    }
}
