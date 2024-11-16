using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerDamage : MonoBehaviour, IDamageMethod
{
    [SerializeField] private Collider FireTrigger;
    [SerializeField] private ParticleSystem FireEffect;
    [HideInInspector] public float Damage;
    [HideInInspector] public float Firerate;

    public void Init(float Damage, float Firerate)
    {
        this.Damage = Damage;
        this.Firerate = Firerate;
    }

    public void DamageTick(Enemy Target)
    {
        FireTrigger.enabled = Target != null;

        if(Target)
        {
            if(!FireEffect.isPlaying) FireEffect.Play();
            return;
        }
        FireEffect.Stop();
    }
}
