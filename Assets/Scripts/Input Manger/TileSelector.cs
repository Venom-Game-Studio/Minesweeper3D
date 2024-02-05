using UnityEngine;

namespace Fabwelt.Common
{
    public class TileSelector : MonoBehaviour
    {
        void Update()
        {
            if (GameManager.isGameOver) return;

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
                        if (GameManager.GameState != Enums.GameState.Start)
                            GameManager.GameState = Enums.GameState.Start;

                        TilePrefabData _brickScript = hit.collider.GetComponentInParent<TilePrefabData>();
                        _brickScript.OpenTile();
                    }
                    else
                    {
                        Debug.LogError(hit.collider.gameObject.name);
                    }
                }
            }

            if (GameManager.GameState != Enums.GameState.Start) return;

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
}