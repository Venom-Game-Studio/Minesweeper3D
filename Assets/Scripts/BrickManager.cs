using TMPro;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public Transform _tileModel;
    public Transform _flagModel;

    [SerializeField] TMP_Text _tileText;

    public void SetTileRotation()
    {
        _tileModel.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 4) * 90, 0);
    }
}