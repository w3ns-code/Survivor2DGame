using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunk;
    public GameObject player;
    public float checkerRadius;
    Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    PlayerMove pm;

    public GameObject currentChunck;

    public List<GameObject> spawnedChuncks;
    GameObject latestChunk;
    public float maxOpDist;
    float opDist;

    float OptimizerCooldown;
    public float OptimizerCooldownDur;


  [System.Obsolete]
  void Start()
    {
        pm = FindObjectOfType<PlayerMove>();
    }

    
    void Update()
    {
        ChunkChecker();
        chunckOptimizer();
    }

    void ChunkChecker()
    {
        if (!currentChunck)
        {
            return;
        }

        if (pm.moveDir.x > 0 && pm.moveDir.y == 0) //Droite
        {
            if(!Physics2D.OverlapCircle(currentChunck.transform.Find("Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunck.transform.Find("Right").position;
                SpawnChunck();
            }
        } else if (pm.moveDir.x < 0 && pm.moveDir.y == 0) //Gauche
        {
            if(!Physics2D.OverlapCircle(currentChunck.transform.Find("Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunck.transform.Find("Left").position;
                SpawnChunck();
            }
        } else if (pm.moveDir.x == 0 && pm.moveDir.y > 0) //Haut
        {
            if(!Physics2D.OverlapCircle(currentChunck.transform.Find("Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunck.transform.Find("Up").position;
                SpawnChunck();
            }
        } else if (pm.moveDir.x == 0 && pm.moveDir.y < 0) //Bas
        {
            if(!Physics2D.OverlapCircle(currentChunck.transform.Find("Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunck.transform.Find("Down").position;
                SpawnChunck();
            }
        } else if (pm.moveDir.x > 0 && pm.moveDir.y > 0) //Droite Haut
        {
            if(!Physics2D.OverlapCircle(currentChunck.transform.Find("RightUp").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunck.transform.Find("RightUp").position;
                SpawnChunck();
            }
        } else if (pm.moveDir.x > 0 && pm.moveDir.y < 0) //Droite Bas
        {
            if(!Physics2D.OverlapCircle(currentChunck.transform.Find("RightDown").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunck.transform.Find("RightDown").position;
                SpawnChunck();
            }
        } else if (pm.moveDir.x < 0 && pm.moveDir.y > 0) //Gauche Haut
        {
            if(!Physics2D.OverlapCircle(currentChunck.transform.Find("LeftUp").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunck.transform.Find("LeftUp").position;
                SpawnChunck();
            }
        } else if (pm.moveDir.x < 0 && pm.moveDir.y < 0) //Gauche Bas
        {
            if(!Physics2D.OverlapCircle(currentChunck.transform.Find("LeftDown").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunck.transform.Find("LeftDown").position;
                SpawnChunck();
            }
        }

        
    }

    void SpawnChunck()
    {
        int rand = Random.Range(0, terrainChunk.Count);
        latestChunk = Instantiate(terrainChunk[rand], noTerrainPosition, Quaternion.identity);
        spawnedChuncks.Add(latestChunk);
    }

    void chunckOptimizer()
    {
        OptimizerCooldown -= Time.deltaTime;

        if(OptimizerCooldown <= 0f)
        {
            OptimizerCooldown = OptimizerCooldownDur; 
        }
        else
        {
            return;
        }

        foreach(GameObject chunk in spawnedChuncks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if(opDist > maxOpDist)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
