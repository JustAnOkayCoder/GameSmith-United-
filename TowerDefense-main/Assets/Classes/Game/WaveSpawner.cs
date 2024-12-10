using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float timeBetweenSpawns = 1f;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private int totalWaves = 3;
    private int currentWave = 0;
    private bool isSpawning = false;
    private GameLoopManager gameLoopManager;

    void Start()
    {
        gameLoopManager = FindObjectOfType<GameLoopManager>();
        EntitySummoner.Init(); // Initialize enemy spawning system
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave < totalWaves)
        {
            if (!isSpawning)
            {
                isSpawning = true;
                currentWave++;
                Debug.Log($"Starting Wave {currentWave}");

                for (int i = 0; i < enemiesPerWave; i++)
                {
                    GameLoopManager.EnqueueEnemyIDToSummon(1); // Queue enemy spawn using ID 1
                    yield return new WaitForSeconds(timeBetweenSpawns);
                }

                enemiesPerWave += 2;
                isSpawning = false;
                yield return new WaitForSeconds(timeBetweenWaves);
            }
            yield return null;
        }
        Debug.Log("All waves completed!");
    }

    public bool IsLastWave() => currentWave >= totalWaves;
    public int GetCurrentWave() => currentWave;
    public int GetTotalWaves() => totalWaves;
}