using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private LayerMask PlacementCheckMask;
    [SerializeField] private LayerMask PlacementCollideMask;
    [SerializeField] private PlayerStats PlayerStatistics;
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
            //RaycastHit hitInfo;

            // Only check collisions on valid placement layers, ignoring the player layer
            if (Physics.Raycast(camray, out RaycastHit hitInfo, 100f, PlacementCollideMask & ~IgnorePlayerMask))
            {
                // Update the tower position to where the ray hits the surface
                CurrentPlacingTower.transform.position = hitInfo.point;

                // Rotate the tower to match the surface's angle, if necessary
                CurrentPlacingTower.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Destroy(CurrentPlacingTower);
                CurrentPlacingTower = null;
                return;
            }

            // Place the tower when left mouse button is clicked
            if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
            {
                if (!hitInfo.collider.CompareTag("CantPlace"))
                {
                    BoxCollider TowerCollider = CurrentPlacingTower.GetComponent<BoxCollider>();
                    TowerCollider.isTrigger = true;

                    Vector3 Center = CurrentPlacingTower.transform.position + TowerCollider.center;
                    Vector3 HalfExtents = TowerCollider.size / 2f;

                    // Check if there's space to place the tower (this also ignores triggers to avoid false collisions)
                    if (!Physics.CheckBox(Center, HalfExtents, Quaternion.identity, PlacementCheckMask, QueryTriggerInteraction.Ignore))
                    {
                        TowerBehaviour CurrentTowerBehaviour = CurrentPlacingTower.GetComponent<TowerBehaviour>();
                        GameLoopManager.TowersInGame.Add(CurrentTowerBehaviour);

                        PlayerStatistics.AddMoney(-CurrentTowerBehaviour.SummonCost);

                        TowerCollider.isTrigger = false;
                        CurrentPlacingTower = null;  // Reset after placement
                    }
                }
                else
                {
                    Debug.LogWarning("No BoxCollider found on " + CurrentPlacingTower.name);
                }
            }
        }
    }


    // Method to set the tower to place
    public void SetTowerToPlace(GameObject tower)
    {
        int TowerSummonCost = tower.GetComponent<TowerBehaviour>().SummonCost;

        if (PlayerStatistics.GetMoney() >= TowerSummonCost)
        {
            CurrentPlacingTower = Instantiate(tower, Vector3.zero, Quaternion.identity);

        }
        else
        {
            Debug.Log("You need more money to build " + tower.name);
        }

    }

    public void UpgradeTower(GameObject tower)
    {
        // Get the TowerBehaviour component of the specified tower
        TowerBehaviour towerBehaviour = tower.GetComponent<TowerBehaviour>();

        if (towerBehaviour == null)
        {
            Debug.LogWarning("Tower does not have a TowerBehaviour component.");
            return;
        }

        // Check if the player has enough money to upgrade
        int upgradeCost = towerBehaviour.UpgradeCost;
        if (PlayerStatistics.GetMoney() >= upgradeCost)
        {
            // Deduct the upgrade cost and upgrade the tower
            PlayerStatistics.AddMoney(-upgradeCost);
            towerBehaviour.Upgrade();

            Debug.Log("Upgraded " + tower.name + " to level " + towerBehaviour.UpgradeLevel);
        }
        else
        {
            Debug.Log("Not enough money to upgrade " + tower.name);
        }
    }


    //int TowerUpgradeCost;

}

