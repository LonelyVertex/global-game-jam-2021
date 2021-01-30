using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    [CreateAssetMenu]
    public class GameConfiguration : ScriptableObject
    {
        [Serializable]
        public class LevelConfiguration
        {
            [SerializeField] int gridSize;
            [SerializeField] int minTiles;
            [SerializeField] int maxTiles;
            [SerializeField] int boxCount;
            [SerializeField] int mineCount;

            public int GridSize => gridSize;
            public int MinTiles => minTiles;
            public int MaxTiles => maxTiles;
            public int BoxCount => boxCount;
            public int MineCount => mineCount;
        }

        [SerializeField] List<LevelConfiguration> levels;
        public List<LevelConfiguration> Levels => levels;
    }
}