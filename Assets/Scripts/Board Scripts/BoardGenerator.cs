using UnityEngine;
using Fabwelt.Common;
using Fabwelt.Common.Enums;
using System.Collections.Generic;
using Fabwelt.Managers.Scriptable;

namespace Fabwelt.Managers.Board
{
    public class BoardGenerator : MonoBehaviour
    {
        public static BoardGenerator instance;

        [SerializeField] TilePrefabData _tilePrefab;
        [SerializeField] Transform _tileParent;

        internal List<TilePrefabData> bricks = new List<TilePrefabData>();

        List<Vector2Int> _coordinates = new List<Vector2Int>();

        private void Awake()
        {
            instance = this;

            GameManager.SelectedLevel = GameManager.instance.LevelCatalog.Levels.Find(x => x.LevelDifficulty == GameManager.instance.levelDifficulty);
        }

        private void Start()
        {
            Camera.main.orthographicSize = GameManager.SelectedLevel.cameraZoom;

            Generate();
        }

        public void Generate()
        {
            SpawnTiles();
            BoardManager.instance.GenerateBoard();
        }

        void SpawnTiles()
        {
            RemoveOldTiles();
            GenerateCoordinates(GameManager.SelectedLevel.size.y, GameManager.SelectedLevel.size.x);
            PlaceTileOnCoordinates();
        }

        void RemoveOldTiles()
        {
#if UNITY_EDITOR
            foreach (Transform t in _tileParent) DestroyImmediate(t.gameObject);
#else
            foreach (Transform t in _tileParent) Destroy(t.gameObject);
#endif
        }

        void GenerateCoordinates(int x, int y)
        {
            _coordinates = new List<Vector2Int>();
            for (int j = y / 2; j >= -y / 2; j--)
                for (int i = -x / 2; i <= x / 2; i++)
                    _coordinates.Add(new Vector2Int(i, j));
        }

        void PlaceTileOnCoordinates()
        {
            bricks = new List<TilePrefabData>();
            int i = 1;
            foreach (Vector2Int co in _coordinates)
            {
                TilePrefabData _go = Instantiate(_tilePrefab, new Vector3(co.x, _tileParent.position.y, co.y), Quaternion.identity, _tileParent);
                _go.name = $"Tile_{i}";
                i++;

                bricks.Add(_go);
            }
        }






#if UNITY_EDITOR
        [Space(20f), Header("For Editor Only")]
        [SerializeField] LevelDifficulty levelDifficulty;
        [SerializeField] LevelCatalog LevelCatalog;

        [ContextMenu("Generate Board")]
        void Gen()
        {
            GameManager.SelectedLevel = LevelCatalog.Levels.Find(x => x.LevelDifficulty == levelDifficulty);

            Camera.main.orthographicSize = GameManager.SelectedLevel.cameraZoom;

            SpawnTiles();
        }
#endif
    }
}