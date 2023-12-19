using TMPro;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public Transform _tileModel;
    public Transform _flagModel;

    [SerializeField] TMP_Text _tileText;
    public string GetTileText { get { return _tileText.text; } }

    public void SetTileRotation()
    {
        _tileModel.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 4) * 90, 0);
    }

    public void SetTileText(string _text)
    {
        _tileText.text = _text;
    }
}