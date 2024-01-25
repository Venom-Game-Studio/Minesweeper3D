using System;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    public List<BrickManager> bricks = new List<BrickManager>();

    public string[,] _board = new string[9, 9];
    List<string> _mines = new List<string>();

    private void Awake()
    {
        instance = this;
    }

    [ContextMenu("Generate")]
    void GenerateBoard()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
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
        } while (_mines < 10);
    }

    void CalculateMines()
    {
        try
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (_board[i, j] == "*")
                        continue;

                    int _mines = 0;

                    if (i - 1 >= 0 && j - 1 >= 0 && string.Equals(_board[i - 1, j - 1], "*")) _mines++;
                    if (i - 1 >= 0 && j >= 0 && string.Equals(_board[i - 1, j], "*")) _mines++;
                    if (i - 1 >= 0 && j + 1 < 9 && string.Equals(_board[i - 1, j + 1], "*")) _mines++;
                    if (i >= 0 && j - 1 >= 0 && string.Equals(_board[i, j - 1], "*")) _mines++;
                    if (i >= 0 && j + 1 < 9 && string.Equals(_board[i, j + 1], "*")) _mines++;
                    if (i + 1 < 9 && j - 1 >= 0 && string.Equals(_board[i + 1, j - 1], "*")) _mines++;
                    if (i + 1 < 9 && j >= 0 && string.Equals(_board[i + 1, j], "*")) _mines++;
                    if (i + 1 < 9 && j + 1 < 9 && string.Equals(_board[i + 1, j + 1], "*")) _mines++;

                    _board[i, j] = _mines.ToString();
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
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                int _index = (9 * i) + j;
                bricks[_index].SetTileText(_board[i, j]);
            }
        }
    }

    void PrintArr()
    {
        string _str = string.Empty;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
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

            int _index = (9 * x) + y;

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
        //CheckNativeTile(x, y, y);
        //CheckNativeTileRev(x, y, y);

        OpenEmptyMines();
    }

    [SerializeField] List<int> _tileToOpen = new List<int>();

    void CheckClickTile(int x, int y)
    {
        int _index = (9 * x) + y;
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








































    void CheckNativeTile(int i, int j, int y)
    {
        int _index = 0;
        try
        {
            _index = (9 * i) + j;
            Debug.Log($"--> {i},{j} -> {_index}");
            if (_index < _board.Length)
            {
                if (_board[i, j].Equals("0"))
                {

                    _tileToOpen.Add(_index);

                    CheckNativeTile(i, j + 1, y);
                }
                else if (i + 1 < 9 && j + 1 < 9 && _board[i + 1, y].Equals("0"))
                {
                    j = y;
                    Debug.Log(i + " " + j);
                    CheckNativeTile(i + 1, j, y);
                }

                //CheckNativeTile(i + 1, j);
                //AddBorderTileToOpen(i, j);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

    }

    void CheckNativeTileRev(int i, int j, int y)
    {
        int _index = 0;
        try
        {
            _index = (9 * i) + j;
            Debug.Log($"--> {i},{j} -> {_index}");
            if (_index < _board.Length)
            {
                if (i - 1 >= 0 && j - 1 >= 0 && _board[i, j].Equals("0"))
                {
                    _tileToOpen.Add(_index);

                    CheckNativeTileRev(i, j - 1, y);
                }
                else if (i - 1 >= 0 && j - 1 >= 0 && _board[i - 1, y].Equals("0"))
                {
                    j = y;
                    Debug.Log(i + " " + j);
                    CheckNativeTileRev(i - 1, j, y);
                }

                //CheckNativeTile(i + 1, j);
                //AddBorderTileToOpen(i, j);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

    }

    void AddBorderTileToOpen(int i, int j)
    {
        int _index = (9 * i) + j;
        _tileToOpen.Add(_index);
    }


    void OpenEmptyMines()
    {
        foreach (int i in _tileToOpen)
            bricks[i]._tileModel.gameObject.SetActive(false);
    }





    bool CheckNativeTiles(int i, int j)
    {






        Debug.LogWarning($"{_board.Length}");
        Debug.LogWarning($"{_board[i, j] == null}");
        Debug.LogWarning($"{_board[i, j + 1] == null}");

        if (!IsConnected(_board[i, j], _board[i, j + 1])) return false;
        Debug.LogWarning("A");
        //down check from i,j
        if (!IsConnected(_board[i, j], _board[i + 1, j])) return false;
        Debug.LogWarning("B");
        //up and left i+1, j+1
        i += 1;
        j += 1;

        if (!IsConnected(_board[i, j], _board[i - 1, j])) return false;
        Debug.LogWarning("C");
        if (!IsConnected(_board[i, j], _board[i, j - 1])) return false;
        Debug.LogWarning("D");

        return true;
    }

    bool IsConnected(string dot1, string dot2)
    {
        return dot1.Contains(dot2);
    }


















    (int, int) GetRandomPosition()
    {
        int x = UnityEngine.Random.Range(0, 9);
        int y = UnityEngine.Random.Range(0, 9);

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
        int x = _brickIndex / 9;
        int y = _brickIndex % 9;
        return (x, y);
    }
}
