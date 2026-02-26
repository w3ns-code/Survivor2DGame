using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindFirstObjectByType<PlayerMove>().transform;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime); //Ennemie move au player en tout temps
    }
}
