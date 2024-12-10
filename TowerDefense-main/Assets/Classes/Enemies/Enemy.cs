using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public List<Effect> ActiveEffects;
    public float DamageResistance = 1f;
    public float MaxHealth;
    public float Health;
    public float Speed = 10f;
    public int ID;
    public Transform RootPart;
    public int NodeIndex;
    public int damageToPlayer = 10;

    public void Init()
    {
        ActiveEffects = new List<Effect>();
        Health = MaxHealth;
        transform.position = GameLoopManager.NodePositions[0];
        NodeIndex = 0;
    }

    private void Update()
    {
        Tick();

        // Handle movement
        if (NodeIndex < GameLoopManager.NodePositions.Length)
        {
            Vector3 targetPosition = GameLoopManager.NodePositions[NodeIndex];
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * Speed * Time.deltaTime;

            // Check if reached node
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                NodeIndex++;
                if (NodeIndex >= GameLoopManager.NodePositions.Length)
                {
                    ReachEnd();
                }
            }
        }
    }

    public void Tick()
    {
        for (int i = 0; i < ActiveEffects.Count; i++)
        {
            if (ActiveEffects[i].ExpireTime > 0f)
            {
                if (ActiveEffects[i].DamageDelay > 0f)
                {
                    ActiveEffects[i].DamageDelay -= Time.deltaTime;
                }
                else
                {
                    GameLoopManager.EnqueueDamageData(new EnemyDamageData(this, ActiveEffects[i].Damage, 1f));
                    ActiveEffects[i].DamageDelay = 1f / ActiveEffects[i].DamageRate;
                }
                ActiveEffects[i].ExpireTime -= Time.deltaTime;
            }
        }
        ActiveEffects.RemoveAll(x => x.ExpireTime <= 0f);
    }

    private void ReachEnd()
    {
        // Damage player before removing enemy
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DamagePlayer(damageToPlayer);
        }

        // Properly remove enemy using EntitySummoner
        EntitySummoner.RemoveEnemy(this);
        // Don't call Destroy(gameObject) - EntitySummoner handles this
    }
}
