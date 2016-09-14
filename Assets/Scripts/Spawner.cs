using UnityEngine;
using System.Collections;
using System;

public class Spawner : MonoBehaviour {

    public bool devMode;

    public GameObject[] spawnPoints;
    public Enemy[] enemies;

    int currentWaveNumber;
    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    int timeBetweenSpawns;
    float nextSpawnTime;

    bool isDisabled;

    public event Action<int> OnNewWave;

    void Start()
    {
        currentWaveNumber = 0;
        FindObjectOfType<Player>().OnDeath += OnPlayerDeath;

        NextWave();
    }

    void Update()
    {
        if (!isDisabled)
        {
            if ((enemiesRemainingToSpawn > 0) && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + timeBetweenSpawns;

                SpawnEnemy();
            }
        }

        if (devMode)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                {
                    GameObject.Destroy(enemy.gameObject);
                }
                NextWave();
            }
        }
    }

    void SpawnEnemy()
    {
        Transform spawnLocation = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length - 1)].transform;

        Enemy spawnedEnemy = Instantiate(enemies[UnityEngine.Random.Range(0, enemies.Length - 1)], spawnLocation.position + Vector3.up, Quaternion.identity) as Enemy;
        spawnedEnemy.OnDeath += OnEnemyDeath;
    }

    void OnPlayerDeath()
    {
        isDisabled = true;
    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;

        if (enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        currentWaveNumber++;

        enemiesRemainingToSpawn = currentWaveNumber * 10;
        enemiesRemainingAlive = enemiesRemainingToSpawn;

        timeBetweenSpawns = 5 / currentWaveNumber;

        if (OnNewWave != null)
        {
            OnNewWave(currentWaveNumber);
        }
    }
}
