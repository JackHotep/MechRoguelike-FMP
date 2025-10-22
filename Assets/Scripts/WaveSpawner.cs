using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public int currentWave;
    private int waveValue;
    public List<GameObject> enemiesSpawning = new List<GameObject>();

    public List<Transform> spawnPoints = new List<Transform>();
    public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;
    
    void Start()
    {
        GenerateWave();
    }

    
    void FixedUpdate()
    {
        if (spawnTimer <= 0)
        {
            if (enemiesSpawning.Count > 0)
            {
                Instantiate(enemiesSpawning[0], spawnPoints[Random.Range(0, spawnPoints.Count)].position,
                    Quaternion.identity);
                enemiesSpawning.RemoveAt(0);
                spawnTimer = spawnInterval;
            }
            else
            {
                waveTimer = 0;
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }
    }

    public void GenerateWave()
    {
        waveValue = Random.Range(3+(currentWave * 2), 5 + (currentWave * 3));
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesSpawning.Count;
        waveTimer = waveDuration;
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0)
        {
            int randomEnemyID = Random.Range(0, enemies.Count);
            int randomEnemyCost = enemies[randomEnemyID].cost;

            if (waveValue - randomEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randomEnemyID].enemyPrefab);
                waveValue -= randomEnemyCost;
            }
            else if (waveValue < 0)
            {
                break;
            }
        }
        enemiesSpawning.Clear();
        enemiesSpawning = generatedEnemies;
    }
}

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}