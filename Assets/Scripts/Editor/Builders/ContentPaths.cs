namespace TestingLab.EditorTools.Builders
{
    /// <summary>Where generated assets live. Tests read these too, so nothing is hard-coded twice.</summary>
    public static class ContentPaths
    {
        public const string ScenesFolder = "Assets/Scenes";
        public const string MaterialsFolder = "Assets/Art/Materials";
        public const string PrefabsFolder = "Assets/Prefabs";
        public const string DataFolder = "Assets/Data";
        public const string LevelsFolder = "Assets/Data/Levels";

        public const string LevelCatalog = DataFolder + "/LevelCatalog.asset";
        public const string ShopCatalog = DataFolder + "/ShopCatalog.asset";
        public const string CoinPrefab = PrefabsFolder + "/Coin.prefab";
        public const string EnemyPrefab = PrefabsFolder + "/Enemy.prefab";

        public static string ScenePath(string sceneName) => $"{ScenesFolder}/{sceneName}.unity";
    }
}
