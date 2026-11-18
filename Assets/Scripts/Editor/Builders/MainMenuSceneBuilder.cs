using TestingLab.Gameplay.Config;
using TestingLab.Gameplay.Flow;
using TestingLab.Gameplay.Infrastructure;
using TestingLab.Gameplay.UI;
using UnityEditor;
using UnityEngine;

namespace TestingLab.EditorTools.Builders
{
    internal static class MainMenuSceneBuilder
    {
        public static void Build()
        {
            var scene = SceneBuilderBase.NewScene();
            var levels = SceneBuilderBase.Load<LevelCatalog>(ContentPaths.LevelCatalog);
            var coinPrefab = SceneBuilderBase.Load<GameObject>(ContentPaths.CoinPrefab);
            var camera = Camera.main;
            camera.transform.SetPositionAndRotation(new Vector3(0f, 1.2f, -6f), Quaternion.identity);

            // A few big spinning coins behind the menu, just for looks.
            for (int i = 0; i < 5; i++)
            {
                var coin = (GameObject)PrefabUtility.InstantiatePrefab(coinPrefab);
                coin.name = $"Decor Coin {i + 1}";
                coin.transform.position = new Vector3(-6f + i * 3f, 1.5f + (i % 2) * 1.5f, 4f);
                coin.transform.localScale = new Vector3(1.6f, 0.1f, 1.6f);
            }

            var canvas = UiFactory.CreateCanvas("Menu Canvas");
            UiFactory.EnsureEventSystem();

            var title = UiFactory.CreateText(canvas.transform, "Title", "COIN RUSH", UiTheme.TitleSize + 32, color: UiTheme.Accent);
            title.fontStyle = FontStyle.Bold;
            UiFactory.Place(title.rectTransform, new Vector2(0.5f, 0.86f), new Vector2(1200f, 140f));
            var subtitle = UiFactory.CreateText(canvas.transform, "Subtitle", "a Testing Lab game", UiTheme.BodySize, color: UiTheme.MutedText);
            UiFactory.Place(subtitle.rectTransform, new Vector2(0.5f, 0.77f), new Vector2(800f, 50f));

            var menu = SceneBuilderBase.CreateDialog(canvas.transform, "Menu", new Vector2(520f, 420f));
            var play = UiFactory.CreateButton(menu, "PlayButton", "New Run");
            var resume = UiFactory.CreateButton(menu, "ContinueButton", "Continue");
            var quit = UiFactory.CreateButton(menu, "QuitButton", "Quit", UiTheme.DangerButton);
            var best = UiFactory.CreateText(menu, "BestScore", "Best score: 0", UiTheme.BodySize, color: UiTheme.Accent);
            UiFactory.SetPreferredSize(best, height: 44f);

            var message = UiFactory.CreateText(canvas.transform, "Message", "", UiTheme.SmallSize, color: UiTheme.MutedText);
            UiFactory.Place(message.rectTransform, new Vector2(0.5f, 0.08f), new Vector2(1400f, 60f));

            var view = new GameObject("MainMenuView").AddComponent<MainMenuView>();
            Wiring.Set(view, "playButton", play);
            Wiring.Set(view, "continueButton", resume);
            Wiring.Set(view, "continueLabel", resume.GetComponentInChildren<UnityEngine.UI.Text>());
            Wiring.Set(view, "quitButton", quit);
            Wiring.Set(view, "bestScoreText", best);
            Wiring.Set(view, "messageText", message);

            var controller = new GameObject("MainMenuController").AddComponent<MainMenuController>();
            Wiring.Set(controller, "levels", levels);
            Wiring.Set(controller, "view", view);

            SceneBuilderBase.AddLabNotes("Main Menu", LabNotesText.MainMenu);
            SceneBuilderBase.Save(scene, SceneNames.MainMenu);
        }
    }
}
