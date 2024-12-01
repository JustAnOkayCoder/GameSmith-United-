using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections; // For NativeArray, Allocator
using Unity.Jobs;        // For JobHandle, IJobParallelForTransform
using UnityEngine.Jobs;
using UnityEditor;  // For TransformAccessArray



public class GameLoopManager : MonoBehaviour
{
    public static List<TowerBehaviour> TowersInGame;//targeting
    public static float[] NodeDistances;//for tower targeting
    private static Queue<int> EnemyIDsToSummon;
    public bool LoopShouldEnd;

    private PlayerStats PlayerStatistics;

    private static Queue<ApplyEffectData> EffectsQueue;//flamethrower stuff
    private static Queue<EnemyDamageData> DamageData;
    private static Queue<Enemy> EnemiesToRemove;
    public static Vector3[] NodePositions; //positions of our nodes on the board
    public Transform NodeParent;

    // Wave System all added code will have a WS some how added to it 
    // incase it messes up our loop or something we will be able to find it easier.

    //WS variables

    public int totalWaves = 3; // change this for win case
    public int currentWave = 0; // should track waves
    public int enemiesPerWave = 5; // enemies per wave
    public float timeBetweenWaves = 10f; // should be 10 seconds
    private bool waveInProgress = false; // is there a current wave

    public GameObject victoryScreen;


    // Start is called before the first frame update
    void Start()
    {
        PlayerStatistics = FindObjectOfType<PlayerStats>();
        EffectsQueue = new Queue<ApplyEffectData>();
        DamageData = new Queue<EnemyDamageData>();
        TowersInGame = new List<TowerBehaviour>();
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

        NodeDistances = new float[NodePositions.Length-1];
        for (int i = 0; i < NodeDistances.Length; i++)
        {
            NodeDistances[i] = Vector3.Distance(NodePositions[i], NodePositions[i+1]);
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
        //WS if the wave ends start a new one
        if (!waveInProgress && EntitySummoner.EnemiesInGame.Count == 0)
        {
            StartWave();
        }
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
        
        foreach(TowerBehaviour tower in TowersInGame)
        {
            tower.Target = TowerTargeting.GetTarget(tower, TowerTargeting.TargetType.Last);
            tower.Tick();
        }

        //Apply Effects
        
        if(EffectsQueue.Count > 0)
        {
            for(int i = 0; i < EffectsQueue.Count; i++)
            {
                ApplyEffectData CurrentDamageData = EffectsQueue.Dequeue();
                Effect EffectDuplicate = CurrentDamageData.EnemyToAffect.ActiveEffects.Find(x => x.EffectName == CurrentDamageData.EffectToApply.EffectName);

                if(EffectDuplicate == null)
                {
                    CurrentDamageData.EnemyToAffect.ActiveEffects.Add(CurrentDamageData.EffectToApply);
                }
                else
                {
                    EffectDuplicate.ExpireTime = CurrentDamageData.EffectToApply.ExpireTime;
                }
            }
        }

        //Tick Enemies

        foreach(Enemy CurrentEnemy in EntitySummoner.EnemiesInGame)
        {
            CurrentEnemy.Tick();
        }

        //Damage Enemies

        if(DamageData.Count > 0)
        {
            for(int i = 0; i < DamageData.Count; i++)
            {
                EnemyDamageData CurrentDamageData = DamageData.Dequeue();
                CurrentDamageData.TargetedEnemy.Health -= CurrentDamageData.TotalDamage / CurrentDamageData.Resistance;
                PlayerStatistics.AddMoney((int)CurrentDamageData.TotalDamage); //gives us money each time you cause damage

                if (CurrentDamageData.TargetedEnemy.Health <= 0f)
                {
                    if (!EnemiesToRemove.Contains(CurrentDamageData.TargetedEnemy))
                    {
                        EnqueueEnemyToRemove(CurrentDamageData.TargetedEnemy);
                    }
                }
            }
        }

        //Remove Enemies

        if (EnemiesToRemove.Count > 0)
        {
            for (int i = 0; i < EnemiesToRemove.Count; i++)
            {
                EntitySummoner.RemoveEnemy(EnemiesToRemove.Dequeue());
            }
        }

        // WS check for the win
        CheckForWinCondition();

        //Remove Towers

        yield return null;
    }
   }

   //WS start wave
   void StartWave()
   {
    if (currentWave < totalWaves)
    {
        currentWave++;
        StartCoroutine(SpawnWave(enemiesPerWave));
    }
   }

   //WS enemies spawning in waves

   IEnumerator SpawnWave(int numberOfEnemies)
   {
    waveInProgress = true;
    for (int i = 0; i < numberOfEnemies; i++)
    {
        EnqueueEnemyIDToSummon(1);//type of enemy to spawn
        yield return new WaitForSeconds(1f); // should be delay between spawns
    }
    waveInProgress = false;
   }

   //WS did you win

   void CheckForWinCondition()
   {
    if (currentWave >= totalWaves && EntitySummoner.EnemiesInGame.Count == 0)
    {
        Victory();
    }
   }

   //WS Victory

   void Victory()
   {
    Debug.Log("Victory! You have beaten all of the waves");
    if (victoryScreen != null)
    {
        victoryScreen.SetActive(true); // this should display the you won UI
    }
    Time.timeScale = 0; // should pause the game I think
   }


   public static void EnqueueEffectToApply(ApplyEffectData effectData)
   {
    EffectsQueue.Enqueue(effectData);
   }

   public static void EnqueueDamageData(EnemyDamageData damagedata)
   {
    DamageData.Enqueue(damagedata);
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

public class Effect
{
    public Effect(string effectName, float damageRate, float damage, float expireTime)
    {
        EffectName = effectName;
        Damage = damage;
        DamageRate = damageRate;
        ExpireTime = expireTime;

    }

    public string EffectName;
    public float DamageDelay;//time after the initial impact until it damages
    public float DamageRate;//how much damage per second
    public float Damage;//damage of the fire
    public float ExpireTime;//how long the fire lasts

}

public struct ApplyEffectData
{
    public ApplyEffectData(Enemy enemyToAffect, Effect effectToApply)
    {
        EnemyToAffect = enemyToAffect;
        EffectToApply = effectToApply;
    }

    public Enemy EnemyToAffect;
    public Effect EffectToApply;
}
public struct EnemyDamageData
{
    public EnemyDamageData(Enemy target, float damage, float resistance)
    {
        TargetedEnemy = target;
        TotalDamage = damage;
        Resistance = resistance;
    }
    public Enemy TargetedEnemy;
    public float TotalDamage;
    public float Resistance;
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
