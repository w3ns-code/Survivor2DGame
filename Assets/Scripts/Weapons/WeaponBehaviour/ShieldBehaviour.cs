using System.Collections.Generic;
using UnityEngine;

public class ShielBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> markedEnemies;
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && !markedEnemies.Contains(col.gameObject)){
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage());

            markedEnemies.Add(col.gameObject);
        }
        else if (col.CompareTag("Props"))
        {
            if(col.gameObject.TryGetComponent(out BreakableProps breakable) && !markedEnemies.Contains(col.gameObject))
            {
                breakable.TakeDamage(GetCurrentDamage());
                markedEnemies.Add(col.gameObject);
            }
        }
    }
}
