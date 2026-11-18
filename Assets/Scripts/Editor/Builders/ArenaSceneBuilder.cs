using TestingLab.Gameplay.Combat;
using TestingLab.Gameplay.Config;
using TestingLab.Gameplay.Flow;
using TestingLab.Gameplay.Infrastructure;
using TestingLab.Gameplay.Pickups;
using TestingLab.Gameplay.Player;
using TestingLab.Gameplay.Spawning;
using TestingLab.Gameplay.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TestingLab.EditorTools.Builders
{
    internal static class ArenaSceneBuilder
    {
        private const float ArenaHalfSize = 9f;

        public static void Build()
        {
            var scene = SceneBuilderBase.NewScene();
            var levels = SceneBuilderBase.Load<LevelCatalog>(ContentPaths.LevelCatalog);
            var coinPrefab = SceneBuilderBase.Load<GameObject>(ContentPaths.CoinPrefab);
            var enemyPrefab = SceneBuilderBase.Load<GameObject>(ContentPaths.EnemyPrefab);
            var camera = Camera.main;
            camera.transform.position = new Vector3(0f, 19f, -13f);
            camera.transform.LookAt(new Vector3(0f, 0f, -1f));
            camera.fieldOfView = 55f;

            BuildFloorAndWalls();
            var player = BuildPlayer();

            var spawner = new GameObject("Wave Spawner").AddComponent<WaveSpawner>();
            Wiring.Set(spawner, "coinPrefab", coinPrefab);
            Wiring.Set(spawner, "enemyPrefab", enemyPrefab);

            var canvas = UiFactory.CreateCanvas("HUD Canvas");
            UiFactory.EnsureEventSystem();
            var hud = BuildHud(canvas.transform);
            var pause = BuildPauseView(canvas.transform);
            var results = BuildResultsView(canvas.transform);

            var arena = new GameObject("Arena Controller").AddComponent<ArenaController>();
            Wiring.Set(arena, "levels", levels);
            Wiring.Set(arena, "player", player);
            Wiring.Set(arena, "spawner", spawner);
            Wiring.Set(arena, "hud", hud);
            Wiring.Set(arena, "pauseView", pause);
            Wiring.Set(arena, "resultsView", results);

            SceneBuilderBase.AddLabNotes("Arena", LabNotesText.Arena);
            SceneBuilderBase.Save(scene, SceneNames.Arena);
        }

        private static void BuildFloorAndWalls()
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(2f, 1f, 2f); // 20 x 20 units
            ground.GetComponent<Renderer>().sharedMaterial = MaterialLibrary.Ground;

            var walls = new GameObject("Walls").transform;
            float edge = ArenaHalfSize + 1f;
            CreateWall(walls, "Wall North", new Vector3(0f, 0.5f, edge), new Vector3(2f * edge + 1f, 1f, 1f));
            CreateWall(walls, "Wall South", new Vector3(0f, 0.5f, -edge), new Vector3(2f * edge + 1f, 1f, 1f));
            CreateWall(walls, "Wall East", new Vector3(edge, 0.5f, 0f), new Vector3(1f, 1f, 2f * edge + 1f));
            CreateWall(walls, "Wall West", new Vector3(-edge, 0.5f, 0f), new Vector3(1f, 1f, 2f * edge + 1f));
        }

        private static void CreateWall(Transform parent, string name, Vector3 position, Vector3 scale)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.SetParent(parent, false);
            wall.transform.position = position;
            wall.transform.localScale = scale;
            wall.GetComponent<Renderer>().sharedMaterial = MaterialLibrary.Wall;
        }

        private static PlayerController BuildPlayer()
        {
            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.tag = "Player";
            player.transform.position = new Vector3(0f, 1f, 0f);
            player.GetComponent<Renderer>().sharedMaterial = MaterialLibrary.Player;
            PrefabBuilder.AddEyes(player.transform);

            var body = player.AddComponent<Rigidbody>();
            body.isKinematic = true;
            body.useGravity = false;

            player.AddComponent<HealthComponent>();
            player.AddComponent<DamageReceiver>();
            player.AddComponent<Collector>();
            var controller = player.AddComponent<PlayerController>();
            controller.ArenaHalfExtents = new Vector2(ArenaHalfSize, ArenaHalfSize);
            return controller;
        }

        private static HudView BuildHud(Transform canvas)
        {
            var root = UiFactory.CreateRect(canvas, "HUD");
            UiFactory.Stretch(root);

            var level = UiFactory.CreateText(root, "Level", "Level 1", UiTheme.BodySize, color: UiTheme.MutedText);
            UiFactory.Place(level.rectTransform, new Vector2(0.5f, 1f), new Vector2(900f, 50f), new Vector2(0f, -16f));

            var score = UiFactory.CreateText(root, "Score", "Score 0", UiTheme.HeadingSize, TextAnchor.UpperLeft, UiTheme.Accent);
            UiFactory.Place(score.rectTransform, new Vector2(0f, 1f), new Vector2(600f, 56f), new Vector2(32f, -24f));

            var coins = UiFactory.CreateText(root, "Coins", "Coins 0/0", UiTheme.BodySize, TextAnchor.UpperLeft);
            UiFactory.Place(coins.rectTransform, new Vector2(0f, 1f), new Vector2(600f, 44f), new Vector2(32f, -84f));

            var timer = UiFactory.CreateText(root, "Timer", "1:00", UiTheme.TitleSize, TextAnchor.UpperRight);
            UiFactory.Place(timer.rectTransform, new Vector2(1f, 1f), new Vector2(300f, 80f), new Vector2(-120f, -16f));

            var pauseButton = UiFactory.CreateButton(root, "PauseButton", "II", fontSize: UiTheme.HeadingSize);
            UiFactory.Place((RectTransform)pauseButton.transform, new Vector2(1f, 1f), new Vector2(72f, 72f), new Vector2(-28f, -20f));

            var healthBack = UiFactory.CreatePanel(root, "Health Bar", UiTheme.HealthBack);
            UiFactory.Place(healthBack.rectTransform, new Vector2(0f, 0f), new Vector2(460f, 44f), new Vector2(32f, 32f));
            var healthFill = UiFactory.CreatePanel(healthBack.transform, "Fill", UiTheme.HealthFill);
            UiFactory.Stretch(healthFill.rectTransform, 4f);
            healthFill.rectTransform.pivot = new Vector2(0f, 0.5f);
            var healthText = UiFactory.CreateText(healthBack.transform, "Value", "100/100", UiTheme.SmallSize);
            UiFactory.Stretch(healthText.rectTransform);

            var damage = UiFactory.CreateText(root, "Damage Popup", "-10", UiTheme.TitleSize, color: UiTheme.DangerButton);
            damage.fontStyle = FontStyle.Bold;
            UiFactory.Place(damage.rectTransform, new Vector2(0.5f, 0.62f), new Vector2(600f, 100f));

            var view = root.gameObject.AddComponent<HudView>();
            Wiring.Set(view, "levelText", level);
            Wiring.Set(view, "scoreText", score);
            Wiring.Set(view, "coinsText", coins);
            Wiring.Set(view, "timerText", timer);
            Wiring.Set(view, "healthText", healthText);
            Wiring.Set(view, "healthFill", healthFill.rectTransform);
            Wiring.Set(view, "damageText", damage);
            Wiring.Set(view, "pauseButton", pauseButton);
            return view;
        }

        private static PauseView BuildPauseView(Transform canvas)
        {
            var root = UiFactory.CreateRect(canvas, "Pause");
            UiFactory.Stretch(root);
            var overlay = SceneBuilderBase.CreateOverlay(root, "Panel");
            var dialog = SceneBuilderBase.CreateDialog(overlay.transform, "Dialog", new Vector2(520f, 360f));
            SceneBuilderBase.Heading(dialog, "Title", "Paused");
            var resume = UiFactory.CreateButton(dialog, "ResumeButton", "Resume");
            var quit = UiFactory.CreateButton(dialog, "QuitButton", "Main Menu", UiTheme.DangerButton);

            var view = root.gameObject.AddComponent<PauseView>();
            Wiring.Set(view, "panel", overlay);
            Wiring.Set(view, "resumeButton", resume);
            Wiring.Set(view, "quitButton", quit);
            return view;
        }

        private static ResultsView BuildResultsView(Transform canvas)
        {
            var root = UiFactory.CreateRect(canvas, "Results");
            UiFactory.Stretch(root);
            var overlay = SceneBuilderBase.CreateOverlay(root, "Panel");
            var dialog = SceneBuilderBase.CreateDialog(overlay.transform, "Dialog", new Vector2(640f, 560f));
            var title = SceneBuilderBase.Heading(dialog, "Title", "Level Complete!", UiTheme.TitleSize);
            title.color = UiTheme.Accent;
            var summary = UiFactory.CreateText(dialog, "Summary", "Score 0", UiTheme.BodySize);
            UiFactory.SetPreferredSize(summary, height: 90f);
            var next = UiFactory.CreateButton(dialog, "ContinueButton", "To the Shop");
            var retry = UiFactory.CreateButton(dialog, "RetryButton", "Retry");
            var menu = UiFactory.CreateButton(dialog, "MenuButton", "Main Menu", UiTheme.DangerButton);

            var view = root.gameObject.AddComponent<ResultsView>();
            Wiring.Set(view, "panel", overlay);
            Wiring.Set(view, "titleText", title);
            Wiring.Set(view, "summaryText", summary);
            Wiring.Set(view, "continueButton", next);
            Wiring.Set(view, "continueLabel", next.GetComponentInChildren<Text>());
            Wiring.Set(view, "retryButton", retry);
            Wiring.Set(view, "menuButton", menu);
            return view;
        }
    }
}
