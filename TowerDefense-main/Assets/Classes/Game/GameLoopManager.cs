using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections; // For NativeArray, Allocator
using Unity.Jobs;        // For JobHandle, IJobParallelForTransform
using UnityEngine.Jobs;  // For TransformAccessArray



public class GameLoopManager : MonoBehaviour
{
    private static Queue<int> EnemyIDsToSummon;
    public bool LoopShouldEnd;

    private static Queue<Enemy> EnemiesToRemove;
    public static Vector3[] NodePositions; //positions of our nodes on the board
    public Transform NodeParent;


    // Start is called before the first frame update
    void Start()
    {
        EntitySummoner.Init();
        EnemyIDsToSummon = new Queue<int>();
        EnemiesToRemove = new Queue<Enemy>();

        NodePositions = new Vector3[NodeParent.childCount];// starts at main node

        // Log the number of child nodes
    Debug.Log("NodeParent child count: " + NodeParent.childCount);

        for(int i = 0; i < NodePositions.Length; i++)
        {
            NodePositions[i] = NodeParent.GetChild(i).position;
        }

        StartCoroutine(GameLoop());
        InvokeRepeating("SummonTest", 0f, 1f);
        
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

        NativeArray<Vector3> NodesToUse = new NativeArray<Vector3>(NodePositions, Allocator.TempJob);
        NativeArray<int> NodeIndices = new NativeArray<int>(EntitySummoner.EnemiesInGame.Count, Allocator.TempJob);
        NativeArray<float> EnemySpeeds = new NativeArray<float>(EntitySummoner.EnemiesInGame.Count, Allocator.TempJob);
        TransformAccessArray EnemyAccess = new TransformAccessArray(EntitySummoner.EnemiesInGameTransform.ToArray(), 2);

                //populate nodeindecies and enemyspeed
        for(int i = 0; i < EntitySummoner.EnemiesInGame.Count; i++)
        {
            EnemySpeeds[i] = EntitySummoner.EnemiesInGame[i].Speed;
            NodeIndices[i] = EntitySummoner.EnemiesInGame[i].NodeIndex;
        }

        MoveEnemiesJob MoveJob = new MoveEnemiesJob
        {
            NodePositions = NodesToUse,
            EnemySpeed = EnemySpeeds,
            NodeIndex = NodeIndices,
            deltaTime = Time.deltaTime
        };

        JobHandle MoveJobHandle = MoveJob.Schedule(EnemyAccess);
        MoveJobHandle.Complete();

        //set indexes the job handle modified
        for(int i = 0; i < EntitySummoner.EnemiesInGame.Count; i++)
        {
            EntitySummoner.EnemiesInGame[i].NodeIndex = NodeIndices[i];

            if(EntitySummoner.EnemiesInGame[i].NodeIndex == NodePositions.Length)
            {
                EnqueueEnemyToRemove(EntitySummoner.EnemiesInGame[i]);
            }
        }
                // doing this helps use manage the arrays without using actual code
        NodesToUse.Dispose();
        EnemySpeeds.Dispose();
        NodeIndices.Dispose();
        EnemyAccess.Dispose();
        //Tick Towers

        //Apply Effects

        //Damage Enemies

        //Remove Enemies

        if(EnemiesToRemove.Count > 0)
        {
            for(int i = 0; i < EnemiesToRemove.Count; i++)
            {
                EntitySummoner.RemoveEnemy(EnemiesToRemove.Dequeue());
            }
        }

        //Remove Towers

        yield return null;
    }
   }

   public static void EnqueueEnemyIDToSummon(int ID)
   {
    EnemyIDsToSummon.Enqueue(ID);
   }

   public static void EnqueueEnemyToRemove(Enemy EnemyToRemove)
   {
        EnemiesToRemove.Enqueue(EnemyToRemove);
   }
}

public struct MoveEnemiesJob : IJobParallelForTransform
{
    [NativeDisableParallelForRestriction]
    public NativeArray<int> NodeIndex;
    [NativeDisableParallelForRestriction]
        public NativeArray<float> EnemySpeed;
        [NativeDisableParallelForRestriction]
    public NativeArray<Vector3> NodePositions;
    public float deltaTime;
    //execute the movements according to the nodes
    public void Execute(int index, TransformAccess transform)
    {
        if(NodeIndex[index] < NodePositions.Length - 1)
        {

        
        Vector3 PositionToMoveTo = NodePositions[NodeIndex[index]];
        transform.position = Vector3.MoveTowards(transform.position, PositionToMoveTo, EnemySpeed[index] * deltaTime);

        if(transform.position == PositionToMoveTo)
        {
            NodeIndex[index]++;
        }//keeps moving through the different positions
        }
    }
}
