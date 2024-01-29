using System;
using UnityEngine;
using Fabwelt.Common;
using System.Collections.Generic;
using Fabwelt.Managers.Scriptable;

namespace Fabwelt.Managers.Board
{
    public class BoardGenerator : MonoBehaviour
    {
        [SerializeField] LevelCatalog _levelCatalog;
        [SerializeField] BrickManager _tilePrefab;

        [SerializeField] Level _selectedLevel;
        public List<BrickManager> bricks = new List<BrickManager>();

        List<Vector2Int> _coordinates = new List<Vector2Int>();

        private void Start()
        {
            _selectedLevel = _levelCatalog.Levels.Find(x => x.LevelDifficulty == GameManager.instance.levelDifficulty);

            Camera.main.orthographicSize = _selectedLevel.cameraZoom;

            Generate();
        }

        [ContextMenu("Generate Board")]
        public void Generate()
        {

            SpawnTiles();
            GenerateBoard();
        }

        void SpawnTiles()
        {
            RemoveOldTiles();
            GenerateCoordinates(_selectedLevel.size.y, _selectedLevel.size.x);
            PlaceTileOnCoordinates();
        }

        #region Tile Spawning
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
            bricks = new List<BrickManager>();
            int i = 1;
            foreach (Vector2Int co in _coordinates)
            {
                BrickManager _go = Instantiate(_tilePrefab, new Vector3(co.x, this.transform.position.y, co.y), Quaternion.identity, this.transform);
                _go.name = $"Tile_{i}";
                i++;

                bricks.Add(_go);
            }
        }
        #endregion

        #region Gameplay Generation

        public string[,] _board;
        [SerializeField] List<string> _mines = new List<string>();


        void GenerateBoard()
        {
            _board = new string[_selectedLevel.size.x, _selectedLevel.size.y];

            for (int i = 0; i < _selectedLevel.size.x; i++)
            {
                for (int j = 0; j < _selectedLevel.size.y; j++)
                {
                    _board[i, j] = "0";
                }
            }

            SpawnMines();
            CalculateMines();

            SetupBricks();
            PrintArr();
        }

        void SpawnMines()
        {

            int _mines = 0;
            do
            {
                (int x, int y) = GetRandomPosition();

                if (string.Equals(_board[x, y], "0"))
                {
                    _board[x, y] = "*";
                    this._mines.Add(string.Format("{0},{1}", x, y));
                    _mines++;
                }
            } while (_mines < _selectedLevel.mineCount);
        }

        void CalculateMines()
        {
            try
            {
                for (int i = 0; i < _selectedLevel.size.x; i++)
                {
                    for (int j = 0; j < _selectedLevel.size.y; j++)
                    {
                        if (_board[i, j] == "*")
                            continue;

                        int _mines = 0;

                        if (i - 1 >= 0 && j - 1 >= 0 && string.Equals(_board[i - 1, j - 1], "*")) _mines++;
                        if (i - 1 >= 0 && j >= 0 && string.Equals(_board[i - 1, j], "*")) _mines++;
                        if (i - 1 >= 0 && j + 1 < _selectedLevel.size.y && string.Equals(_board[i - 1, j + 1], "*")) _mines++;
                        if (i >= 0 && j - 1 >= 0 && string.Equals(_board[i, j - 1], "*")) _mines++;
                        if (i >= 0 && j + 1 < _selectedLevel.size.y && string.Equals(_board[i, j + 1], "*")) _mines++;
                        if (i + 1 < _selectedLevel.size.x && j - 1 >= 0 && string.Equals(_board[i + 1, j - 1], "*")) _mines++;
                        if (i + 1 < _selectedLevel.size.x && j >= 0 && string.Equals(_board[i + 1, j], "*")) _mines++;
                        if (i + 1 < _selectedLevel.size.x && j + 1 < _selectedLevel.size.y && string.Equals(_board[i + 1, j + 1], "*")) _mines++;

                        _board[i, j] = _mines.ToString();
                    }
                }

                //PrintArr();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        void SetupBricks()
        {
            for (int i = 0; i < _selectedLevel.size.x; i++)
            {
                for (int j = 0; j < _selectedLevel.size.y; j++)
                {
                    int _index = (_selectedLevel.size.y * i) + j;
                    bricks[_index].SetTileText(_board[i, j]);
                }
            }
        }

        [ContextMenu("Print")]
        void PrintArr()
        {
            string _str = string.Empty;
            for (int i = 0; i < _selectedLevel.size.x; i++)
            {
                for (int j = 0; j < _selectedLevel.size.y; j++)
                {
                    _str += string.Format("{0}\t", _board[i, j]);
                }
                _str += "\n";
            }
            Debug.Log("\n" + _str);
        }






















        [ContextMenu("RevealMines")]
        public void RevealMines()
        {
            for (int i = 0; i < _mines.Count; i++)
            {
                (int x, int y) = GetPosition(i);

                int _index = (_selectedLevel.size.y * x) + y;

                bricks[_index]._tileModel.gameObject.SetActive(false);
            }
        }

        public void RevealEmptyMines(BrickManager _brick)
        {
            int _brickIndex = bricks.FindIndex((x) => x == _brick);

            (int x, int y) = GetPositionFromIndex(_brickIndex);

            Debug.Log(string.Format("{0},{1}", x, y));

            _tileToOpen = new List<int>();

            CheckClickTile(x, y);

            OpenEmptyMines();
        }

        [SerializeField] List<int> _tileToOpen = new List<int>();

        void CheckClickTile(int x, int y)
        {
            int _index = (_selectedLevel.size.x * x) + y;
            Debug.Log($"index : {_index} -> ({x},{y})");
            Debug.Log($"[{_board[x, y]}]");

            if (_tileToOpen.Contains(_index))
            {
                Debug.Log($"{_index} already exist!!!");
                return;
            }

            if (_board[x, y].Equals("0"))
            {
                _tileToOpen.Add(_index);

                CheckTileUp(x - 1, y);
                CheckTileDown(x + 1, y);
                CheckTileLeft(x, y - 1);
                CheckTileRight(x, y + 1);
            }
            else
            {
                if (!_board[x, y].Equals("*"))
                    _tileToOpen.Add(_index);
            }
        }

        void CheckTileUp(int x, int y)
        {
            if (x < 0) return;

            CheckClickTile(x, y);
        }
        void CheckTileDown(int x, int y)
        {
            if (x >= 9) return;

            CheckClickTile(x, y);
        }
        void CheckTileLeft(int x, int y)
        {
            if (y < 0) return;

            CheckClickTile(x, y);
        }
        void CheckTileRight(int x, int y)
        {
            if (y >= 9) return;

            CheckClickTile(x, y);
        }

        void OpenEmptyMines()
        {
            foreach (int i in _tileToOpen)
                bricks[i]._tileModel.gameObject.SetActive(false);
        }













        (int, int) GetRandomPosition()
        {
            int x = UnityEngine.Random.Range(0, _selectedLevel.size.x);
            int y = UnityEngine.Random.Range(0, _selectedLevel.size.y);

            return (x, y);
        }

        (int, int) GetPosition(int i)
        {
            int x = int.Parse((_mines[i].Split(','))[0]);
            int y = int.Parse((_mines[i].Split(','))[1]);
            return (x, y);
        }

        (int, int) GetPositionFromIndex(int _brickIndex)
        {
            int x = _brickIndex / _selectedLevel.size.x;
            int y = _brickIndex % _selectedLevel.size.y;
            return (x, y);
        }
        #endregion
    }
}