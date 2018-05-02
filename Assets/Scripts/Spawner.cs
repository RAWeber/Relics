using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

    public bool devMode;

    public GameObject[] spawnPoints;
    public EnemySpawn[] enemyList;
    List<EnemySpawn> availableEnemies = new List<EnemySpawn>();
    List<EnemySpawn> enemiesToSpawn = new List<EnemySpawn>();

    int currentWaveNumber;

    int enemiesRemainingAlive;
    float timeBetweenSpawns;
    float nextSpawnTime;

    int maxWaveCost;
    int spawnCostTotal;
    int currentSpawnIndex;
    float totalSpawnRate;

    bool isDisabled;

    public event Action<int> OnNewWave;

    void Start()
    {
        currentWaveNumber = 0;
        //FindObjectOfType<Player>().OnDeath += OnPlayerDeath;

        enemyList = enemyList.OrderBy(m => m.waveEncounter).ToArray<EnemySpawn>();

        SendWave();
    }

    void Update()
    {
        if (!isDisabled)
        {
            if(enemiesToSpawn.Count > 0)
            {
                SpawnEnemies();
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
                SendWave();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                isDisabled = true;
            }
        }
    }

    void SendWave()
    {
        currentWaveNumber++;
        maxWaveCost = currentWaveNumber * 5;
        print(currentWaveNumber / 10);
        timeBetweenSpawns = 1 - (currentWaveNumber / 10) * 0.1f;

        UpdateAvailableEnemies();
        CreateSpawnList();

        if (OnNewWave != null)
        {
            OnNewWave(currentWaveNumber);
        }
    }

    void UpdateAvailableEnemies()
    {
        while (currentSpawnIndex < enemyList.Length && enemyList[currentSpawnIndex].waveEncounter <= currentWaveNumber)
        {
            availableEnemies.Add(enemyList[currentSpawnIndex]);
            totalSpawnRate += enemyList[currentSpawnIndex].spawnRate;
            currentSpawnIndex++;
        }
    }

    void CreateSpawnList()
    {
        enemiesToSpawn.Clear();
        spawnCostTotal = 0;
        while(spawnCostTotal < maxWaveCost)
        {
            int enemyIndex = GetRandomSpawnIndex();
            EnemySpawn spawn = availableEnemies[enemyIndex];
            enemiesToSpawn.Add(spawn);
            spawnCostTotal += spawn.cost;
        }

        enemiesRemainingAlive = enemiesToSpawn.Count;
    }

    int GetRandomSpawnIndex()
    {
        float val = Mathf.Floor(UnityEngine.Random.value * totalSpawnRate);
        for(int i = 0; i < availableEnemies.Count; i++)
        {
            val -= availableEnemies[i].spawnRate;
            if(val < 0)
            {
                return i;
            }
        }
        return availableEnemies.Count - 1;
    }

    void SpawnEnemies()
    {
        if (Time.time > nextSpawnTime)
        {
            Transform spawnLocation = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform;
            Enemy spawnedEnemy = Instantiate(enemiesToSpawn[0].enemy, spawnLocation.position + Vector3.up, Quaternion.identity) as Enemy;

            enemiesToSpawn.RemoveAt(0);
            spawnedEnemy.OnDeath += OnEnemyDeath;

            nextSpawnTime = Time.time + timeBetweenSpawns;
        }
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
            SendWave();
        }
    }
}

[Serializable]
public struct EnemySpawn
{
    public Enemy enemy;
    public int cost;
    public int waveEncounter;
    public float spawnRate;
}
