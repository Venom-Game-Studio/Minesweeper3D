using Fabwelt.Common;
using TMPro;
using UnityEngine;

public class TilePrefabData : MonoBehaviour
{
    public Transform _tileModel;
    public Transform _flagModel;
    public Transform _mineModel;

    [SerializeField] TMP_Text _tileText;
    public string GetTileText { get { return _tileText.text; } }
    [SerializeField] bool isMine = false;
    public bool isFlaged = false;

    public void SetTileRotation()
    {
        _tileModel.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 4) * 90, 0);
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
            if (!string.IsNullOrEmpty(_text))
                _tileText.color = GameManager.instance.ColorScheme.colors[int.Parse(_text) - 1];
        }
    }

    public void OpenTile()
    {
        if (isMine)
        {
            BoardManager.instance.RevealMines();
            return;
        }

        BoardManager.instance.RevealEmptyMines(this);
    }

    public void PlaceFlag()
    {
        isFlaged = !isFlaged;
        _flagModel.gameObject.SetActive(isFlaged);
    }
}