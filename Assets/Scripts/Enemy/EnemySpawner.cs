using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class EnemySpawner : MonoBehaviour
{
   
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroup;
        public int waveQuota;
        public float spawnInterval;
        public int spawnCount;
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public int spawnCount;
        public GameObject enemyPrefab;
    }
    public List<Wave> waves;
    public int currentWaveCount;

    float spawnTimer;
    public int enemiesAlive;
    public int maxEnnemiesAllowed;
    public bool maxEnnemiesLimit = false;
    public float waveInterval;

    public List<Transform> relativeSpawnPoint;
    Transform player;

    void Start()
    {
        player = FindFirstObjectByType<PlayerStats>().transform;
        CalculateWaveQuota();
    }

    void Update()
    {
        if(currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0)
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;

        if(spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnedEnnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        //Attendre l'intervale de 1 minute avant de passer pour l'autre wave d'ennemies
        yield return new WaitForSeconds(waveInterval);

        if(currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroup)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }

    void SpawnedEnnemies()
    {
        if(waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnnemiesLimit)
        {
            foreach(var enemyGroup in waves[currentWaveCount].enemyGroup)
            {
                if(enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    if(enemiesAlive >= maxEnnemiesAllowed)
                    {
                        maxEnnemiesLimit = true;
                        return;
                    }

                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoint[UnityEngine.Random.Range(0, relativeSpawnPoint.Count)].position, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }
        if(enemiesAlive < maxEnnemiesAllowed)
        {
            maxEnnemiesLimit = false;
        }
    }


    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }
}
