using UnityEngine;

public class BrickSelector : MonoBehaviour
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
                //Debug.Log(hit.collider.gameObject.name);

                //if (hit.collider.gameObject.TryGetComponent(out BrickManager entity))
                if (hit.collider.CompareTag("MineTile"))
                {
                    BrickManager _brickScript = hit.collider.GetComponentInParent<BrickManager>();
                    _brickScript.OpenTile();
                    //Debug.Log(_brickScript.GetTileText);
                    //Debug.Log(entity.GetTileText);
                }
                else
                {
                    Debug.LogError(hit.collider.gameObject.name);
                }
            }
        }
    }
}