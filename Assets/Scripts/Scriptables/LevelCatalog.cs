using System;
using UnityEngine;
using Fabwelt.Common.Enums;
using System.Collections.Generic;

namespace Fabwelt.Managers.Scriptable
{
    [Serializable]
    public class Level
    {
        public LevelDifficulty LevelDifficulty;
        public Vector2Int size;
        public int mineCount;

        public float cameraZoom;
    }

    [CreateAssetMenu(fileName = "Level Catalog", menuName = "Catalog/Level")]
    public class LevelCatalog : ScriptableObject
    {
        public List<Level> Levels;
    }
}