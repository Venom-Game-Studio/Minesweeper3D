using System;
using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public List<BrickManager> bricks = new List<BrickManager>();

    public string[,] _board = new string[9, 9];

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
            (int x, int y) = GetRandPos();

            if (string.Equals(_board[x, y], "0"))
            {
                _board[x, y] = "*";
                _mines++;
            }
        } while (_mines < 10);
    }

    (int, int) GetRandPos()
    {
        int x = UnityEngine.Random.Range(0, 9);
        int y = UnityEngine.Random.Range(0, 9);

        return (x, y);
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
}
