using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour, IDamageMethod
{
    [SerializeField] private Transform LaserPivot;
    [SerializeField] private LineRenderer LaserRenderer;

   private float Damage;
   private float Firerate;
   private float Delay;
    private float upgradecost;

    public void Init(float Damage, float Firerate)
   {
    this.Damage = Damage;
    this.Firerate = Firerate;
    Delay = 1f / Firerate;
        this.upgradecost = 150;
    }

   public void DamageTick(Enemy Target)
   {
        if(Target)
        {
            LaserRenderer.enabled =  true;
            LaserRenderer.SetPosition(0, LaserPivot.position);
            LaserRenderer.SetPosition(1, Target.RootPart.position);
            if (Delay > 0f)
            {
                Delay -= Time.deltaTime;
                return;
            }

            GameLoopManager.EnqueueDamageData(new EnemyDamageData(Target, Damage, Target.DamageResistance));
            Delay = 1f / Firerate;
        }

        LaserRenderer.enabled = false;

        Firerate += 1;
        Damage += 2;
        upgradecost += 200;
    }
}
