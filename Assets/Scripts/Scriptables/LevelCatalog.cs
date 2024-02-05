using System;
using UnityEngine;
using Fabwelt.Common.Enums;
using System.Collections.Generic;

namespace Fabwelt.Managers.Scriptable
{
    [Serializable]
    public class Level
    {
        public string Name;
        public LevelDifficulty LevelDifficulty;
        public Vector2Int size;
        public int mineCount;

        public float cameraZoom;

        public Score score;
    }

    [Serializable]
    public class Score
    {
        public int baseScore;
        [SerializeField] int maxTime;

        public int MaxTime { get { return maxTime * 60; } }
    }

    [CreateAssetMenu(fileName = "Level Catalog", menuName = "Catalog/Level")]
    public class LevelCatalog : ScriptableObject
    {
        public List<Level> Levels;

        public int minePanaltyPoint;
    }
}