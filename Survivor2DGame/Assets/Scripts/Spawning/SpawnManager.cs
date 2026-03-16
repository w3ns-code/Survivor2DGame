using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    // Index de la vague actuelle
    int currentWaveIndex;

    // Combien d’ennemis ont été spawn dans la vague
    int currentWaveSpawnCount = 0;

    // Liste des ennemis présents dans la scène
    List<GameObject> existingSpawns = new List<GameObject>();


    // Données des vagues (ScriptableObjects)
    public WaveData[] data;

    // Caméra utilisée pour spawn hors écran
    public Camera referenceCamera;


    // Nombre maximum d’ennemis pour éviter lag
    public int maximumEnemyCount = 300;


    // Timer avant le prochain spawn
    float spawnTimer;

    // Temps écoulé dans la vague actuelle
    float currentWaveDuration = 0f;


    // Curse augmente le spawn
    public bool boostedByCurse = true;


    // Singleton
    public static SpawnManager instance;



    void Start()
    {
        // Vérifie s'il existe déjà un SpawnManager
        if (instance)
            Debug.LogWarning("Il y a plus d’un SpawnManager dans la scène!");

        instance = this;
    }



    void Update()
    {
        // Diminue le timer
        spawnTimer -= Time.deltaTime;

        // Augmente la durée de la vague
        currentWaveDuration += Time.deltaTime;


        // Si le timer est fini
        if (spawnTimer <= 0)
        {

            // Vérifie si la vague est terminée
            if (HasWaveEnded())
            {
                currentWaveIndex++;

                // reset variables
                currentWaveDuration = 0;
                currentWaveSpawnCount = 0;


                // Si plus de vagues ? stop spawn
                if (currentWaveIndex >= data.Length)
                {
                    Debug.Log("Toutes les vagues sont terminées");
                    enabled = false;
                }

                return;
            }


            // Vérifie si on peut spawn
            if (!CanSpawn())
            {
                ActivateCooldown();
                return;
            }


            // Récupère les ennemis à spawn
            GameObject[] spawns = data[currentWaveIndex].GetSpawns(EnemyStats.count);


            foreach (GameObject prefab in spawns)
            {
                if (!CanSpawn())
                    continue;

                // Spawn l’ennemi
                GameObject enemy = Instantiate(prefab, GeneratePosition(), Quaternion.identity);

                existingSpawns.Add(enemy);

                currentWaveSpawnCount++;
            }

            // Reset timer
            ActivateCooldown();
        }
    }



    // Reset timer spawn
    public void ActivateCooldown()
    {
        float curseBoost = boostedByCurse ? GameManager.GetCumulativeCurse() : 1;

        spawnTimer += data[currentWaveIndex].GetSpawnInterval() / curseBoost;
    }



    // Vérifie si on peut spawn
    public bool CanSpawn()
    {
        if (HasExceededMaxEnemies())
            return false;

        if (currentWaveSpawnCount > data[currentWaveIndex].totalSpawns)
            return false;

        if (currentWaveDuration > data[currentWaveIndex].duration)
            return false;

        return true;
    }



    // Vérifie si trop d’ennemis
    public static bool HasExceededMaxEnemies()
    {
        if (!instance)
            return false;

        return EnemyStats.count > instance.maximumEnemyCount;
    }



    // Vérifie si la vague est terminée
    public bool HasWaveEnded()
    {
        WaveData currentWave = data[currentWaveIndex];


        // Condition durée vague
        if ((currentWave.exitConditions & WaveData.ExitCondition.waveDuration) > 0)
            if (currentWaveDuration < currentWave.duration)
                return false;


        // Condition nombre spawn
        if ((currentWave.exitConditions & WaveData.ExitCondition.reachedTotalSpawns) > 0)
            if (currentWaveSpawnCount < currentWave.totalSpawns)
                return false;


        // Condition tuer tous les ennemis
        existingSpawns.RemoveAll(item => item == null);

        if (currentWave.mustKillAll && existingSpawns.Count > 0)
            return false;

        return true;
    }



    // Génère une position hors écran
    public static Vector3 GeneratePosition()
    {
        if (!instance.referenceCamera)
            instance.referenceCamera = Camera.main;

        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);

        Vector3 viewportPos;


        // Spawn sur un bord de l’écran
        switch (Random.Range(0, 2))
        {
            case 0:
            default:
                viewportPos = new Vector3(Mathf.Round(x), y, instance.referenceCamera.nearClipPlane);
                break;

            case 1:
                viewportPos = new Vector3(x, Mathf.Round(y), instance.referenceCamera.nearClipPlane);
                break;
        }


        // Convertit en position monde
        Vector3 worldPos = instance.referenceCamera.ViewportToWorldPoint(viewportPos);

        worldPos.z = 0;

        return worldPos;
    }



    // Vérifie si un objet est visible à l’écran
    public static bool IsWithinBoundaries(Transform checkedObject)
    {
        Camera c = instance && instance.referenceCamera ? instance.referenceCamera : Camera.main;

        Vector2 viewport = c.WorldToViewportPoint(checkedObject.position);

        if (viewport.x < 0f || viewport.x > 1f)
            return false;

        if (viewport.y < 0f || viewport.y > 1f)
            return false;

        return true;
    }
}