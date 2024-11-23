using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeManager : MonoBehaviour
{
    private TowerBehaviour selectedTower;
    [SerializeField] private PlayerStats playerStatistics;
    [SerializeField] private LayerMask towerLayerMask;

    void Update()
    {
        // Check for mouse click to select a tower
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            SelectTower();
        }

        // Check for the "U" key to upgrade the selected tower
        if (Input.GetKeyDown(KeyCode.U) && selectedTower != null)
        {
            UpgradeSelectedTower();
        }
    }

    void SelectTower()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Only detect hits on the tower layer
        if (Physics.Raycast(ray, out hit, 100f, towerLayerMask))
        {
            TowerBehaviour tower = hit.collider.GetComponent<TowerBehaviour>();

            if (tower != null)
            {
                // Deselect the previous tower
                if (selectedTower != null && selectedTower != tower)
                {
                    HighlightTower(selectedTower, false);
                }

                selectedTower = tower;
                Debug.Log("Selected tower: " + tower.name);

                // Highlight the selected tower
                HighlightTower(selectedTower, true);
            }
        }
        else
        {
            // Deselect the tower if clicking elsewhere
            if (selectedTower != null)
            {
                HighlightTower(selectedTower, false);
                selectedTower = null;
            }

            Debug.Log("No tower selected.");
        }
    }

    void UpgradeSelectedTower()
    {
        int playerMoney = playerStatistics.GetMoney();
        int upgradeCost = selectedTower.UpgradeCost;

        if (playerMoney >= upgradeCost)
        {
            playerStatistics.AddMoney((-upgradeCost) - 100); // Deduct the upgrade cost
            selectedTower.Upgrade();
            Debug.Log($"Upgraded tower: {selectedTower.name} to Level {selectedTower.UpgradeLevel}");
        }
        else
        {
            Debug.Log("Not enough money to upgrade the tower.");
        }
    }

    void HighlightTower(TowerBehaviour tower, bool highlight)
    {
        // Optional: Change the material color or add a visual effect
        Renderer renderer = tower.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = highlight ? Color.green : Color.white;
        }
    }
}