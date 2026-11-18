using System.IO;
using UnityEditor;
using UnityEngine;

namespace TestingLab.EditorTools.Builders
{
    /// <summary>Creates (or reuses) the flat-coloured materials the game uses.</summary>
    internal static class MaterialLibrary
    {
        public static Material Ground => GetOrCreate("Ground", new Color(0.22f, 0.25f, 0.32f));
        public static Material Wall => GetOrCreate("Wall", new Color(0.12f, 0.14f, 0.20f));
        public static Material Player => GetOrCreate("Player", new Color(0.25f, 0.55f, 1f));
        public static Material Enemy => GetOrCreate("Enemy", new Color(0.90f, 0.25f, 0.25f));
        public static Material Coin => GetOrCreate("Coin", new Color(1f, 0.80f, 0.10f), metallic: 0.6f);
        public static Material Accent => GetOrCreate("Accent", new Color(0.95f, 0.95f, 1f));

        private static Material GetOrCreate(string name, Color color, float metallic = 0f)
        {
            string path = $"{ContentPaths.MaterialsFolder}/{name}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                Directory.CreateDirectory(ContentPaths.MaterialsFolder);
                var shader = Shader.Find("Standard");
                if (shader == null)
                    shader = Shader.Find("Universal Render Pipeline/Lit");
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }

            material.color = color;
            if (material.HasProperty("_Metallic"))
                material.SetFloat("_Metallic", metallic);
            if (material.HasProperty("_Glossiness"))
                material.SetFloat("_Glossiness", 0.35f);
            EditorUtility.SetDirty(material);
            return material;
        }
    }
}
