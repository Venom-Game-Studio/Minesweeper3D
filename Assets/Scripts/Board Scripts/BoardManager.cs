using System;
using UnityEngine;
using Fabwelt.Common;
using Fabwelt.Managers.Board;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;
    public string[,] _board;
    [SerializeField] List<string> _mines = new List<string>();

    readonly string _emptySpace = string.Empty;

    int SizeX { get { return GameManager.SelectedLevel.size.x; } }
    int SizeY { get { return GameManager.SelectedLevel.size.y; } }

    private void Awake()
    {
        instance = this;
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

        int _mines = 0;
        do
        {
            (int x, int y) = GetRandomPosition();

            if (string.Equals(_board[x, y], _emptySpace))
            {
                _board[x, y] = "*";
                this._mines.Add(string.Format("{0},{1}", x, y));
                _mines++;
            }
        } while (_mines < GameManager.SelectedLevel.mineCount);
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
                BoardGenerator.bricks[_index].SetTileText(_board[i, j]);
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
        for (int i = 0; i < _mines.Count; i++)
        {
            (int x, int y) = GetPosition(i);

            int _index = (SizeY * x) + y;

            if (BoardGenerator.bricks[_index].isFlaged) continue;
            BoardGenerator.bricks[_index]._tileModel.gameObject.SetActive(false);
        }
    }

    public void RevealEmptyMines(TilePrefabData _brick)
    {
        int _brickIndex = BoardGenerator.bricks.FindIndex((x) => x == _brick);

        (int x, int y) = GetPositionFromIndex(_brickIndex);

        Debug.Log(string.Format("{0},{1}", x, y));

        _tileToOpen = new List<int>();

        CheckClickTile(x, y);

        OpenEmptyTiles();
    }

    [SerializeField] List<int> _tileToOpen = new List<int>();

    void CheckClickTile(int x, int y)
    {
        int _index = (SizeX * x) + y;
        Debug.Log($"index : {_index} -> ({x},{y})");
        Debug.Log($"[{_board[x, y]}]");

        if (_tileToOpen.Contains(_index))
        {
            Debug.Log($"{_index} already exist!!!");
            return;
        }

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
        {
            if (BoardGenerator.bricks[i].isFlaged) continue;
            BoardGenerator.bricks[i]._tileModel.gameObject.SetActive(false);
        }
    }













    (int, int) GetRandomPosition()
    {
        int x = UnityEngine.Random.Range(0, SizeX);
        int y = UnityEngine.Random.Range(0, SizeY);

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
        int x = _brickIndex / SizeX;
        int y = _brickIndex % SizeY;
        return (x, y);
    }






    public void GoBack()
    {
        SceneManager.LoadScene(0);
    }
}
