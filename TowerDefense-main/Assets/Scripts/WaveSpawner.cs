using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;        // The enemy to spawn
    public float timeBetweenWaves = 5f;   // Time between each wave
    public float timeBetweenEnemies = 1f; // Time between each enemy spawn
    public int enemiesPerWave = 5;        // How many enemies per wave
    public Transform spawnPoint;          // Where enemies spawn (first waypoint)

    private int currentWave = 0;
    private float countdown = 2f;

    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        currentWave++;
        UnityEngine.Debug.Log("Wave " + currentWave);  // Fixed the ambiguous reference

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}