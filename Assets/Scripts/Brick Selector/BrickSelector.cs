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
                if (hit.collider.CompareTag("MineTile"))
                {
                    BrickManager _brickScript = hit.collider.GetComponentInParent<BrickManager>();
                    _brickScript.OpenTile();
                }
                else
                {
                    Debug.LogError(hit.collider.gameObject.name);
                }
            }
        }
    }
}