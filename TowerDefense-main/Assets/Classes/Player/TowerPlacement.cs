using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private LayerMask PlacementCheckMask;
    [SerializeField] private LayerMask PlacementCollideMask;
    [SerializeField] private Camera PlayerCamera;
    private GameObject CurrentPlacingTower;
    
    // To avoid placing on the player or any unwanted objects, we add a layer mask to ignore the player
    [SerializeField] private LayerMask IgnorePlayerMask;

    void Start()
    {
    }

    void Update()
    {
        if (CurrentPlacingTower != null)
        {
            // Cast a ray from the camera towards where the mouse is pointing
            Ray camray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit HitInfo;

            // Only check collisions on valid placement layers, ignoring the player layer
            if (Physics.Raycast(camray, out HitInfo, 100f, PlacementCollideMask & ~IgnorePlayerMask))
            {
                // Update the tower position to where the ray hits the surface
                CurrentPlacingTower.transform.position = HitInfo.point;

                // Rotate the tower to match the surface's angle, if necessary
                CurrentPlacingTower.transform.rotation = Quaternion.FromToRotation(Vector3.up, HitInfo.normal);
            }

            // Place the tower when left mouse button is clicked
            if (Input.GetMouseButtonDown(0) && HitInfo.collider != null)
            {
                if (!HitInfo.collider.CompareTag("CantPlace"))
                {
                    BoxCollider TowerCollider = CurrentPlacingTower.GetComponent<BoxCollider>();

                    if (TowerCollider != null)
                    {
                        TowerCollider.isTrigger = true;
                        Vector3 BoxCenter = CurrentPlacingTower.transform.position + TowerCollider.center;
                        Vector3 HalfExtents = TowerCollider.size / 2;

                        // Check if there's space to place the tower (this also ignores triggers to avoid false collisions)
                        if (!Physics.CheckBox(BoxCenter, HalfExtents, Quaternion.identity, PlacementCheckMask, QueryTriggerInteraction.Ignore))
                        {
                            // Finalize placement
                            TowerCollider.isTrigger = false;
                            //CurrentPlacingTower = null;  // Reset after placement
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No BoxCollider found on " + CurrentPlacingTower.name);
                    }
                }
            }
        }
    }

    // Method to set the tower to place
    public void SetTowerToPlace(GameObject tower)
    {
        // Instantiate the tower at a temporary location, or where desired
        CurrentPlacingTower = Instantiate(tower, Vector3.zero, Quaternion.identity);
    }

    
}
