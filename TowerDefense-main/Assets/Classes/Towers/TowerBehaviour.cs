using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public LayerMask EnemiesLayer; // Helps detect enemies
    public Enemy Target;
    public Transform TowerPivot;

    public int SummonCost = 100;

    public float Damage = 10f; // Base damage
    public float Firerate = 1f; // Base firerate
    public float Range = 10f; // Base range
    public float Delay;

    public int UpgradeCost { get; private set; } = 50; // Default cost to upgrade the tower
    public int UpgradeLevel { get; private set; } = 1; // Current upgrade level

    private IDamageMethod CurrentDamageMethodClass;

    // Start is called before the first frame update
    void Start()
    {
        CurrentDamageMethodClass = GetComponent<IDamageMethod>();

        if (CurrentDamageMethodClass == null)
        {
            Debug.LogError("TOWERS No damage class attached to given tower");
        }
        else
        {
            CurrentDamageMethodClass.Init(Damage, Firerate);
        }

        Delay = 1 / Firerate;
    }

    // Update is called once per frame
    public void Tick()
    {
        CurrentDamageMethodClass.DamageTick(Target);

        if (Target != null)
        {
            TowerPivot.transform.rotation = Quaternion.LookRotation(Target.transform.position - transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        if (Target != null)
        {
            Gizmos.DrawWireSphere(transform.position, Range);
            Gizmos.DrawLine(TowerPivot.position, Target.transform.position);
        }
    }

    /// <summary>
    /// Upgrade the tower's attributes.
    /// </summary>
    public void Upgrade()
    {
        UpgradeLevel++; // Increment the upgrade level

        // Upgrade attributes
        Damage *= 1.2f; // Increase damage by 20%
        Firerate *= 1.1f; // Increase firerate by 10%
        Range += 1.0f; // Increase range by 1 unit

        // Update the cost for the next upgrade
        UpgradeCost = 50 * UpgradeLevel;

        Debug.Log($"Tower upgraded to level {UpgradeLevel}: Damage={Damage}, Firerate={Firerate}, Range={Range}, Next UpgradeCost={UpgradeCost}");
    }

    /// <summary>
    /// Get the cost to upgrade the tower.
    /// </summary>
    public int GetUpgradeCost()
    {
        return UpgradeCost; // Return the current upgrade cost
    }
}
