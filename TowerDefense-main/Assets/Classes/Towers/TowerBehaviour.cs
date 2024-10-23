using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public LayerMask EnemiesLayer; // Helps detect enemies
    public Enemy Target;
    public Transform TowerPivot;

    public int SummonCost = 100;

    public float Damage;
    public float Firerate = 1f; // Ensure this has a default value to avoid divide by zero
    public float Range = 10f;
    public float Delay;

    private IDamageMethod CurrentDamageMethodClass;

    // Start is called before the first frame update
    void Start()
    {
        CurrentDamageMethodClass = GetComponent<IDamageMethod>();
        
        if(CurrentDamageMethodClass == null)
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

       if(Target != null)
       {
        TowerPivot.transform.rotation = Quaternion.LookRotation(Target.transform.position - transform.position);
       }
    }

    private void OnDrawGizmos()
    {
        if(Target != null)
        {
            Gizmos.DrawWireSphere(transform.position, Range);
            Gizmos.DrawLine(TowerPivot.position, Target.transform.position);
        }
    }
}
