using System;
using Fabwelt.UI;
using UnityEngine;
using System.Linq;
using Fabwelt.Common;
using Fabwelt.Common.Enums;
using Fabwelt.Managers.Board;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    [SerializeField] internal List<TilePrefabData> flagedTiles = new List<TilePrefabData>();

    string[,] _board;
    List<int> _tileToOpen = new List<int>();
    List<Vector2Int> _mineMatrix = new List<Vector2Int>();
    readonly string _emptySpace = string.Empty;

    int SizeX { get { return GameManager.SelectedLevel.size.x; } }
    int SizeY { get { return GameManager.SelectedLevel.size.y; } }

    public static event Action<TilePrefabData, GameState> OpenTileEvent = delegate { };

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        GameManager.TileFlagUpdate += _data =>
        {
            if (flagedTiles.Exists(x => x == _data))
                flagedTiles.Remove(_data);
            else
                flagedTiles.Add(_data);

            GameUiManager.Instance.UpdateFlageCount(GameManager.SelectedLevel.mineCount - flagedTiles.Count);
        };
    }

    private void OnDisable()
    {
        GameManager.TileFlagUpdate -= _data => { };
    }

    public void GenerateBoard()
    {
        _board = new string[SizeX, SizeY];

        for (int i = 0; i < SizeX; i++)
            for (int j = 0; j < SizeY; j++)
                _board[i, j] = _emptySpace;

        SpawnMines();
        CalculateMines();

        PrintArr();
        SetupBricks();
    }

    void SpawnMines()
    {
        int _totalMines = 0;
        do
        {
            (int x, int y) = GetRandomPosition();

            if (string.Equals(_board[x, y], _emptySpace))
            {
                _board[x, y] = "*";
                _mineMatrix.Add(new Vector2Int(x, y));
                _totalMines++;
            }
        } while (_totalMines < GameManager.SelectedLevel.mineCount);

        _mineMatrix = _mineMatrix.OrderBy(x => x.x).ThenBy(x => x.y).ToList();
    }

    void CalculateMines()
    {
        try
        {
            for (int i = 0; i < SizeX; i++)
            {
                for (int j = 0; j < SizeY; j++)
                {
                    if (_board[i, j] == "*")
                        continue;

                    int _mines = 0;

                    if (i - 1 >= 0 && j - 1 >= 0 && string.Equals(_board[i - 1, j - 1], "*")) _mines++;
                    if (i - 1 >= 0 && j >= 0 && string.Equals(_board[i - 1, j], "*")) _mines++;
                    if (i - 1 >= 0 && j + 1 < SizeY && string.Equals(_board[i - 1, j + 1], "*")) _mines++;
                    if (i >= 0 && j - 1 >= 0 && string.Equals(_board[i, j - 1], "*")) _mines++;
                    if (i >= 0 && j + 1 < SizeY && string.Equals(_board[i, j + 1], "*")) _mines++;
                    if (i + 1 < SizeX && j - 1 >= 0 && string.Equals(_board[i + 1, j - 1], "*")) _mines++;
                    if (i + 1 < SizeX && j >= 0 && string.Equals(_board[i + 1, j], "*")) _mines++;
                    if (i + 1 < SizeX && j + 1 < SizeY && string.Equals(_board[i + 1, j + 1], "*")) _mines++;

                    if (_mines > 0)
                        _board[i, j] = _mines.ToString();
                    else
                        _board[i, j] = _emptySpace;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    void SetupBricks()
    {
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                int _index = (SizeY * i) + j;
                BoardGenerator.instance.bricks[_index].SetTileText(_board[i, j]);
            }
        }
    }

    [ContextMenu("Print")]
    void PrintArr()
    {
        string _str = string.Empty;
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                _str += string.Format("{0}\t", string.IsNullOrEmpty(_board[i, j]) ? "-" : _board[i, j]);
            }
            _str += "\n";
        }
        Debug.Log("\n" + _str);
    }





















    [ContextMenu("RevealMines")]
    public void RevealMines()
    {
        GameManager.GameState = GameState.End;

        OpenTileEvent(null, GameManager.GameState);
    }

    public void RevealEmptyMines(TilePrefabData _brick)
    {
        int _brickIndex = BoardGenerator.instance.bricks.FindIndex((x) => x == _brick);

        (int x, int y) = GetPositionFromIndex(_brickIndex);

        //Debug.Log(string.Format("{0},{1}", x, y));

        _tileToOpen = new List<int>();

        CheckClickTile(x, y);

        OpenEmptyTiles();
    }


    void CheckClickTile(int x, int y)
    {
        int _index = (SizeY * x) + y;
        //Debug.Log($"index : {_index} -> ({x},{y}) [{_board[x, y]}]");

        if (_tileToOpen.Contains(_index))
            return;

        if (_board[x, y].Equals(_emptySpace))
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
        if (x >= SizeX) return;

        CheckClickTile(x, y);
    }
    void CheckTileLeft(int x, int y)
    {
        if (y < 0) return;

        CheckClickTile(x, y);
    }
    void CheckTileRight(int x, int y)
    {
        if (y >= SizeY) return;

        CheckClickTile(x, y);
    }

    void OpenEmptyTiles()
    {
        foreach (int i in _tileToOpen)
            OpenTileEvent(BoardGenerator.instance.bricks[i], GameManager.GameState);
    }













    (int, int) GetRandomPosition()
    {
        int x = UnityEngine.Random.Range(0, SizeX);
        int y = UnityEngine.Random.Range(0, SizeY);

        return (x, y);
    }

    (int, int) GetPositionFromIndex(int _brickIndex)
    {
        int x = _brickIndex / SizeY;
        int y = _brickIndex % SizeY;
        return (x, y);
    }






    public void GoBack()
    {
        SceneManager.LoadScene(0);
    }
}
