using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopM : MonoBehaviour
{

    private static Queue<int> EnemyIDsToSummon;
    public bool LoopShouldEnd;

    // Start is called before the first frame update
    void Start()
    {
        EnemyIDsToSummon = new Queue<int>();
        EntitySummoner.Init();
    }

    IEnumerator Gameloop()
    {
        while(LoopShouldEnd == false)
        {
            //spawn enemies

            if(EnemyIDsToSummon.Count >0)
            {
                for(int i = 0; i < EnemyIDsToSummon.Count; i++)
                {
                    EntitySummoner.SummonEnemy(EnemyIDsToSummon.Dequeue());
                }
            }

            //spawn towers

            //move enemies 

            //tick towers

            //apply effects

            //damage enemies

            //remove enemies

            //remove towers

            yield return null;
        }
    }

    public static void EnqueueEnemyIDToSummon(int ID)
    {
        EnemyIDsToSummon.Enqueue(ID);
    }
}
