using UnityEngine;
using System.Collections.Generic;

public class EdtScript : MonoBehaviour
{
    public List<BrickManager> _gos = new List<BrickManager>();

    [ContextMenu("Rot")]
    public void RotTiles()
    {
        foreach (BrickManager b in _gos)
        {
            b.SetTileRotation();
        }
    }

    [ContextMenu("Mesh OFF")]
    public void MeshOff()
    {
        foreach (BrickManager b in _gos)
        {
            b._tileModel.gameObject.SetActive(false);
            b._flagModel.gameObject.SetActive(false);
        }
    }

    [ContextMenu("Mesh ON")]
    public void MeshOn()
    {
        foreach (BrickManager b in _gos)
        {
            b._tileModel.gameObject.SetActive(true);
            b._flagModel.gameObject.SetActive(false);
        }
    }

    [ContextMenu("Rand Off")]
    public void RandOff()
    {
        foreach (BrickManager b in _gos)
        {
            if (UnityEngine.Random.Range(0, 100) > 75)
                b._tileModel.gameObject.SetActive(false);


            if (UnityEngine.Random.Range(0, 100) > 75 && b._tileModel.gameObject.activeInHierarchy)
                b._flagModel.gameObject.SetActive(true);
        }
    }
}