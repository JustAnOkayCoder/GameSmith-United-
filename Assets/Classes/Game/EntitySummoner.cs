using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySummoner : MonoBehaviour
{
    public static List<Enemy> EnemiesInGame;
    public static Dictionary<int, GameObject> EnemyPrefabs;
    public static Dictionary<int, Queue<Enemy>> EnemyObjectPools;

    private static bool IsInitialized;


    // Start is called before the first frame update
    public static void Init()
    {
        if(!IsInitialized)
        {
            EnemyPrefabs = new Dictionary<int, GameObject>();
            EnemyObjectPools = new Dictionary<int, Queue<Enemy>>();
            EnemiesInGame = new List<Enemy>();  //keeps track of how many guys are alive

            EnemySummonData[] Enemies = Resources.LoadAll<EnemySummonData>("Enemies");  // this loads the enemy data from the unity folders
            //Debug.Log(Enemies[0].name);

            foreach(EnemySummonData enemy in Enemies)
            {
                EnemyPrefabs.Add(enemy.EnemyID, enemy.EnemyPrefab);
                EnemyObjectPools.Add(enemy.EnemyID, new Queue<Enemy>());
                //makes empty object pools for each enemy
            }
            IsInitialized = true;
        }
        else
        {
            Debug.Log("ENTITYSUMMONER: THIS CLASS IS ALREADY INITIALIZED");
        }
    }

    public static Enemy SummonEnemy(int EnemyID)
    {
        EnemyID SummonedEnemy = null;
        
        if(EnemyPrefabs.ContainsKey(EnemyID))
        {
            Queue<EnemyID> ReferencedQueue = EnemyObjectPools[EnemyID];

            if(ReferencedQueue.Count > 0)
            {
                //Dequeue the enemy and initialize

                SummonedEnemy = ReferencedQueue.Dequeue();
                SummonedEnemy.Init();
            }
            else
            {
                //instantiate new instance of enemy and initialize
                GameObject NewEnemy = Instantiate(EnemyPrefabs[EnemyID], Vector3.zero, Quaternion.identity);
                SummonedEnemy = NewEnemy.GetComponent<EnemyID>();
                SummonedEnemy.Init();
            }
        }
        else 
        {
            Debug.Log("ENTITYSUMMONER: ENEMY WITH ID {EnemyID} DOES NOT EXIST!");
            return null; 
        }


        return SummonedEnemy;
    }

   
}
