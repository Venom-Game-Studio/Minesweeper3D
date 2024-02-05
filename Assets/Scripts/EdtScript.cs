using UnityEngine;
using System.Collections.Generic;

public class EdtScript : MonoBehaviour
{
    /*public List<TilePrefabData> _gos = new List<TilePrefabData>();

    [ContextMenu("Rot")]
    public void RotTiles()
    {
        foreach (TilePrefabData b in _gos)
        {
            b.SetTileRotation();
        }
    }

    [ContextMenu("Mesh OFF")]
    public void MeshOff()
    {
        foreach (TilePrefabData b in _gos)
        {
            b._tileModel.gameObject.SetActive(false);
            b._flagModel.gameObject.SetActive(false);
        }
    }

    [ContextMenu("Mesh ON")]
    public void MeshOn()
    {
        foreach (TilePrefabData b in _gos)
        {
            b._tileModel.gameObject.SetActive(true);
            b._flagModel.gameObject.SetActive(false);
        }
    }

    [ContextMenu("Rand Off")]
    public void RandOff()
    {
        foreach (TilePrefabData b in _gos)
        {
            if (UnityEngine.Random.Range(0, 100) > 75)
                b._tileModel.gameObject.SetActive(false);


            if (UnityEngine.Random.Range(0, 100) > 75 && b._tileModel.gameObject.activeInHierarchy)
                b._flagModel.gameObject.SetActive(true);
        }
    }


    [ContextMenu("Solve")]
    public void Solve()
    {
        foreach (TilePrefabData b in _gos)
        {
            if (string.Equals(b.GetTileText, "*"))
            {
                b._tileModel.gameObject.SetActive(false);
                b._flagModel.gameObject.SetActive(false);
                b._mineModel.gameObject.SetActive(true);
            }

            else
            {
                b._tileModel.gameObject.SetActive(false);
                b._flagModel.gameObject.SetActive(false);
                b._mineModel.gameObject.SetActive(false);
            }
        }
    }

    [ContextMenu("Reset Tile Text")]
    public void ResetTileText()
    {
        foreach (TilePrefabData b in _gos)
        {
            b.SetTileText("0");
            b._mineModel.gameObject.SetActive(false);
        }
    }*/
}