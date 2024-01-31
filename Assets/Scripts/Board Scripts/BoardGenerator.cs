using UnityEngine;
using Fabwelt.Common;
using System.Collections.Generic;

namespace Fabwelt.Managers.Board
{
    public class BoardGenerator : MonoBehaviour
    {
        public static BoardGenerator instance;

        [SerializeField] TilePrefabData _tilePrefab;

        public List<TilePrefabData> bricks = new List<TilePrefabData>();

        List<Vector2Int> _coordinates = new List<Vector2Int>();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            GameManager.SelectedLevel = GameManager.instance.LevelCatalog.Levels.Find(x => x.LevelDifficulty == GameManager.instance.levelDifficulty);

            Camera.main.orthographicSize = GameManager.SelectedLevel.cameraZoom;

            Generate();
        }

        [ContextMenu("Generate Board")]
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
            foreach (Transform t in this.transform) Destroy(t.gameObject);
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
                TilePrefabData _go = Instantiate(_tilePrefab, new Vector3(co.x, this.transform.position.y, co.y), Quaternion.identity, this.transform);
                _go.name = $"Tile_{i}";
                i++;

                bricks.Add(_go);
            }
        }
    }
}