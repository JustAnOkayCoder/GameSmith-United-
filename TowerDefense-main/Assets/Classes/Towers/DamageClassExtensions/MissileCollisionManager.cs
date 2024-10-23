using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollisionManager : MonoBehaviour
{
    [SerializeField] private MissileDamage BaseClass;
    [SerializeField] private ParticleSystem ExplosionSystem;
    [SerializeField] private ParticleSystem MissileSystem;
    [SerializeField] private float ExplosionRadius;
    private List<ParticleCollisionEvent> MissileCollisions;
    // Start is called before the first frame update
    void Start()
    {
        MissileCollisions = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
            MissileSystem.GetCollisionEvents(other, MissileCollisions);

            for(int collisionsevent = 0; collisionsevent < MissileCollisions.Count; collisionsevent++)
            {
                ExplosionSystem.transform.position = MissileCollisions[collisionsevent].intersection;
                ExplosionSystem.Play();

                Collider[] EnimiesInRadius = Physics.OverlapSphere(MissileCollisions[collisionsevent].intersection, ExplosionRadius, BaseClass.EnemiesLayer);

                for(int i = 0; i < EnimiesInRadius.Length; i++)
                {
                    Enemy EnemyToDamage = EntitySummoner.EnemyTransformPairs[EnimiesInRadius[i].transform.parent];
                    EnemyDamageData DamageToApply = new EnemyDamageData(EnemyToDamage, BaseClass.Damage, EnemyToDamage.DamageResistance);
                    GameLoopManager.EnqueueDamageData(DamageToApply);
                }
            }
    }
}
