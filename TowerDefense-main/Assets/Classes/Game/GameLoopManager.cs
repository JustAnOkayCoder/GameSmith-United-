using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    private static Queue<int> EnemyIDsToSummon;
    public bool LoopShouldEnd;


    // Start is called before the first frame update
    void Start()
    {
        EntitySummoner.Init();
        EnemyIDsToSummon = new Queue<int>();

        StartCoroutine(GameLoop());
        InvokeRepeating("SummonTest", 0f, 1f);
        InvokeRepeating("RemoveTest", 0f, 0.5f);
    }

    void RemoveTest()
    {
        if(EntitySummoner.EnemiesInGame.Count > 0)
        {
            EntitySummoner.RemoveEnemy(EntitySummoner.EnemiesInGame[Random.Range(0, EntitySummoner.EnemiesInGame.Count)]);
        }
    }

    void SummonTest()
    {
        EnqueueEnemyIDToSummon(1);
    }

   IEnumerator GameLoop()
   {
    while(LoopShouldEnd == false)
    {
        //Spawn Enemies 
        if(EnemyIDsToSummon.Count > 0)
        {
            for(int i = 0; i < EnemyIDsToSummon.Count; i++)
            {
                EntitySummoner.SummonEnemy(EnemyIDsToSummon.Dequeue());
            }
        }
        //Spawn Towers

        //Move Enemies

        //Tick Towers

        //Apply Effects

        //Damage Enemies

        //Remove Enemies

        //Remove Towers

        yield return null;
    }
   }

   public static void EnqueueEnemyIDToSummon(int ID)
   {
    EnemyIDsToSummon.Enqueue(ID);
   }
}
