using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySummoner : MonoBehaviour
{
    public static Dictionary<Transform, Enemy> EnemyTransformPairs;//Gets rid of all the gets in the missile damage class
    public static Dictionary<int, GameObject> EnemyPrefabs;// used with EnemySummonerData
    public static Dictionary<int, Queue<Enemy>> EnemyObjectPools;//different enemy types from the pools 
    public static List<Transform> EnemiesInGameTransform; //enemy movement

    public static List<Enemy> EnemiesInGame;// keeps track of the enemies alive on the board

    private static bool IsInitialized;

    

    // Start is called before the first frame update
    public static void Init()
    {
        if(!IsInitialized)
        {
            EnemyTransformPairs = new Dictionary<Transform, Enemy>();
            EnemyPrefabs = new Dictionary<int, GameObject>();
            EnemyObjectPools = new Dictionary<int, Queue<Enemy>>();
            EnemiesInGame = new List<Enemy>();
            EnemiesInGameTransform = new List<Transform>();
            EnemySummonData[] Enemies = Resources.LoadAll<EnemySummonData>("Enemies");//this goes to the part in project where the resources are for enemies
            
            foreach(EnemySummonData enemy in Enemies)
            {
                EnemyPrefabs.Add(enemy.EnemyID, enemy.EnemyPrefab);
                EnemyObjectPools.Add(enemy.EnemyID, new Queue<Enemy>());
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
        Enemy SummonedEnemy = null;

        if(EnemyPrefabs.ContainsKey(EnemyID))
        {
            Queue<Enemy> ReferencedQueue = EnemyObjectPools[EnemyID];

            if(ReferencedQueue.Count > 0)
            {
                //Dequeued Enemy and initialize 

                SummonedEnemy = ReferencedQueue.Dequeue();
                SummonedEnemy.Init();//pulls from enemy class
                SummonedEnemy.gameObject.SetActive(true);//deals with adding and deleting enemies
            }
            else
            {
                //instantiate new instance of enemy and initialize
                GameObject NewEnemy = Instantiate(EnemyPrefabs[EnemyID], GameLoopManager.NodePositions[0], Quaternion.identity);
                SummonedEnemy = NewEnemy.GetComponent<Enemy>();
                SummonedEnemy.Init();
            }
        }
        else
        {
            Debug.Log($"EntitySummoner: ENEMY WITH ID {EnemyID} DOES NOT EXIST");
            return null;
        }//checks to see if we have that enemy 

        if(!EnemiesInGame.Contains(SummonedEnemy)) EnemiesInGame.Add(SummonedEnemy);
        if(!EnemiesInGameTransform.Contains(SummonedEnemy.transform))EnemiesInGameTransform.Add(SummonedEnemy.transform);
        if(!EnemyTransformPairs.ContainsKey(SummonedEnemy.transform)) EnemyTransformPairs.Add(SummonedEnemy.transform, SummonedEnemy);
        //gets rid of the duplicates in our list
        SummonedEnemy.ID = EnemyID;
        return SummonedEnemy;
    }

    public static void RemoveEnemy(Enemy EnemyToRemove)
    {
        if (EnemyToRemove != null)
        {
            if (EnemyObjectPools.ContainsKey(EnemyToRemove.ID))
            {
                EnemyObjectPools[EnemyToRemove.ID].Enqueue(EnemyToRemove);
                EnemyToRemove.gameObject.SetActive(false);

                if (EnemyTransformPairs.ContainsKey(EnemyToRemove.transform))
                    EnemyTransformPairs.Remove(EnemyToRemove.transform);

                if (EnemiesInGame.Contains(EnemyToRemove))
                    EnemiesInGame.Remove(EnemyToRemove);

                if (EnemiesInGameTransform.Contains(EnemyToRemove.transform))
                    EnemiesInGameTransform.Remove(EnemyToRemove.transform);
            }
        }
    }
}
