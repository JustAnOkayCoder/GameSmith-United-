using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // this is our guys attributes and ID
    public float MaxHealth;
    public float Health;
    public float Speed;
    public int ID; //this is how we tell our different enemies apart  

    public void Init()
    {
        Health = MaxHealth;
    } //new line
    
}
