using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDamage : MonoBehaviour, IDamageMethod
{
    public LayerMask EnemiesLayer;    
    [SerializeField] private ParticleSystem MissileSystem;
    [SerializeField] private Transform TowerHead;

    private ParticleSystem.MainModule MissileSystemMain;
    public float Damage;
    private float Firerate;
    private float Delay;
    private float upgradecost;
    
   public void Init(float Damage, float Firerate)
   {
        MissileSystemMain = MissileSystem.main;
        this.Damage = Damage;
        this.Firerate = Firerate;
        this.upgradecost = 100;
        Delay = 1f / Firerate;
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

            MissileSystemMain.startRotationX = TowerHead.forward.x;
            MissileSystemMain.startRotationY = TowerHead.forward.y;
            MissileSystemMain.startRotationZ = TowerHead.forward.z;

            MissileSystem.Play();
            Delay = 1f / Firerate;

            Firerate += 1;
            Damage += 2;
            upgradecost += 100;

        }
   }
}
