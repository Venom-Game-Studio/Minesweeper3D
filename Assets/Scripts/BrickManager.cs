using TMPro;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public Transform _tileModel;
    public Transform _flagModel;
    public Transform _mineModel;

    [SerializeField] TMP_Text _tileText;
    public string GetTileText { get { return _tileText.text; } }
    [SerializeField] bool isMine = false;

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
            _tileText.text = _text;
    }

    public void OpenTile()
    {
        if (isMine)
        {
            BoardManager.instance.RevealMines();
            return;
        }

        /*if (GetTileText.Equals("0"))
        {
            BoardManager.instance.RevealEmptyMines(this);
            return;
        }*/

        BoardManager.instance.RevealEmptyMines(this);
        _tileModel.gameObject.SetActive(false);
    }
}