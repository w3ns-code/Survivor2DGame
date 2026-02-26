using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

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

    void Start()
    {
        CalculateWaveQuota();
    }

    void Update()
    {
        
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
        
    }
}
