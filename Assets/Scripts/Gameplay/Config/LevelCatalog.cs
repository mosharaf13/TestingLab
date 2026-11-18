using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestingLab.Gameplay.Config
{
    /// <summary>The ordered list of levels in a run. Lives at Assets/Data/LevelCatalog.asset.</summary>
    [CreateAssetMenu(menuName = "Testing Lab/Level Catalog", fileName = "LevelCatalog")]
    public sealed class LevelCatalog : ScriptableObject
    {
        [SerializeField] private List<LevelConfig> levels = new List<LevelConfig>();

        public int Count => levels.Count;
        public IReadOnlyList<LevelConfig> Levels => levels;

        public LevelConfig Get(int index)
        {
            if (levels.Count == 0)
                throw new InvalidOperationException($"{name} has no levels.");

            return levels[Mathf.Clamp(index, 0, levels.Count - 1)];
        }

        public void Configure(IEnumerable<LevelConfig> orderedLevels) => levels = orderedLevels.ToList();
    }
}
