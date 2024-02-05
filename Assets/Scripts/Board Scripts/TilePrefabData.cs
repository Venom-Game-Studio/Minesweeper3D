using TMPro;
using UnityEngine;
using Fabwelt.Common;
using Fabwelt.Common.Enums;
using System;

public class TilePrefabData : MonoBehaviour
{
    [SerializeField] Transform _tileModel;
    [SerializeField] Transform _flagModel;
    [SerializeField] Transform _mineModel;

    [SerializeField] TMP_Text _tileText;

    [SerializeField] bool isMine = false;
    [SerializeField] bool isFlaged = false;

    public static event Action WrongFlagPlaced=delegate { };

    private void OnEnable()
    {
        BoardManager.OpenTileEvent += OpenTileEventCalled;
    }
    private void OnDisable()
    {
        BoardManager.OpenTileEvent -= OpenTileEventCalled;
    }

    private void OpenTileEventCalled(TilePrefabData _tile, GameState _state)
    {
        if (_state == GameState.Start)
        {
            if (_tile != this) return;
            if (isFlaged) return;

            _tileModel.gameObject.SetActive(false);
        }
        else if (_state == GameState.End)
        {
            if (isFlaged && !isMine)
            {
                WrongFlagPlaced();
                SetTileText("X");
                _mineModel.gameObject.SetActive(true);
                _flagModel.gameObject.SetActive(false);

                _tileModel.gameObject.SetActive(false);
            }

            if (!isFlaged && isMine)
            {
                _tileModel.gameObject.SetActive(false);
            }

        }
    }

    public void SetTileText(string _text)
    {
        if (_text.Equals("*"))
        {
            isMine = true;
            _mineModel.gameObject.SetActive(true);
            _tileText.text = string.Empty;
        }
        else
        {
            _tileText.text = _text;
            try
            {
                if (!string.IsNullOrEmpty(_text))
                    _tileText.color = GameManager.instance.ColorScheme.colors[int.Parse(_text) - 1];
            }
            catch
            {
                _tileText.color = Color.red;
            }
        }
    }

    public void OpenTile()
    {
        if (isMine)
        {
            BoardManager.instance.RevealMines();
            return;
        }

        //Debug.LogWarning(_tileText.text);
        BoardManager.instance.RevealEmptyMines(this);
    }

    public void PlaceFlag()
    {
        isFlaged = !isFlaged;
        _flagModel.gameObject.SetActive(isFlaged);

        GameManager.UpdateTileFlag(this);
    }
}