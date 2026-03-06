using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptObjects enemyData;
    
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistantce = 20f;
    Transform player;

    void Awake()
    {
       currentMoveSpeed = enemyData.MoveSpeed;
       currentHealth = enemyData.MaxHealth;
       currentDamage = enemyData.Damage; 
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerStats>().transform;
    }

    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) >= despawnDistantce)
        {
            ReturnEnemy();
        }
    }

  public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if(currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }

    public void OnDestroy()
    {
        EnemySpawner es = FindFirstObjectByType<EnemySpawner>();
        es.OnEnemyKilled();
    }

    void ReturnEnemy()
    {
        EnemySpawner es = FindFirstObjectByType<EnemySpawner>();
        transform.position = player.position + es.relativeSpawnPoint[Random.Range(0, es.relativeSpawnPoint.Count)].position;
    }

}
