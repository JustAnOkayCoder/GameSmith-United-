using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageMethod
{
    public void DamageTick(Enemy Target);
    public void Init(float Damage, float Firerate);
}

public class StandardDamage : MonoBehaviour, IDamageMethod
{
    private float Damage;
    private float Firerate;
    private float Delay;
    private float upgradecost;

    public void Init(float Damage, float Firerate)
   {
        this.Damage = Damage;
        this.Firerate = Firerate;
        Delay = 1f / Firerate;
        this.upgradecost = 50;
    }// stops you from having to recall damage changes

   public void DamageTick(Enemy Target)
   {
        if(Target)
        {
            if (Delay > 0f)
            {
                Delay -= Time.deltaTime;
                return;
            }

            GameLoopManager.EnqueueDamageData(new EnemyDamageData(Target, Damage, Target.DamageResistance));

            Delay = 1f / Firerate;

            Firerate += 2;
            Damage += 2;
            upgradecost += 50;
        }
   }
}
