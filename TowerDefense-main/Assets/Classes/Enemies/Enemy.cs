using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   public float MaxHealth;
   public float Health;
   public float Speed;
   public int ID;

   public int NodeIndex; // this is for enemy movement so it follows our path


   public void Init()
   {
        Health = MaxHealth;
        transform.position = GameLoopManager.NodePositions[0];
        NodeIndex = 0;
   }
}
