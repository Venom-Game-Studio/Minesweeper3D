using UnityEngine;

public class TileSelector : MonoBehaviour
{
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("MineTile"))
                {
                    //Debug.LogWarning(hit.collider.transform.parent.name);

                    TilePrefabData _brickScript = hit.collider.GetComponentInParent<TilePrefabData>();
                    _brickScript.OpenTile();
                }
                else
                {
                    Debug.LogError(hit.collider.gameObject.name);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("MineTile"))
                {
                    TilePrefabData _brickScript = hit.collider.GetComponentInParent<TilePrefabData>();
                    _brickScript.PlaceFlag();
                }
                else
                {
                    Debug.LogError(hit.collider.gameObject.name);
                }
            }
        }
    }
}