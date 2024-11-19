using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTriggerManager : MonoBehaviour
{
    [SerializeField] private FlamethrowerDamage BaseClass;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Effect FlameEffect = new Effect("Fire", BaseClass.Firerate, BaseClass.Damage, 5f); //makes our fire damage last for 5 seconds
            ApplyEffectData effectData = new ApplyEffectData(EntitySummoner.EnemyTransformPairs[other.transform.parent], FlameEffect);
            GameLoopManager.EnqueueEffectToApply(effectData);
        }
    }
}
