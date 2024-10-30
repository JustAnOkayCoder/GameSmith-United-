using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTriggerManager : MonoBehaviour
{
    [SerializeField] private FlamethrowerDamage BaseClass;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Transform enemyTransform = other.transform; // Directly get the enemy's transform

            // Check if the enemy or its parent is in the dictionary
            if (EntitySummoner.EnemyTransformPairs.ContainsKey(enemyTransform))
            {
                Effect FlameEffect = new Effect("Fire", BaseClass.Firerate, BaseClass.Damage, 5f); //makes our fire damage last for 5 seconds
                ApplyEffectData effectData = new ApplyEffectData(EntitySummoner.EnemyTransformPairs[enemyTransform], FlameEffect);
                GameLoopManager.EnqueueEffectToApply(effectData);
            }
            else
            {
                Debug.LogWarning("The enemy transform is not found in the EnemyTransformPairs dictionary.");
            }
        }
    }
}
