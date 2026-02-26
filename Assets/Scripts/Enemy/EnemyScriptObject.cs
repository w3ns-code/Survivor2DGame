using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObjet", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptObjects : ScriptableObject
{
    [SerializeField]    
    float moveSpeed;
    public float MoveSpeed {get => moveSpeed; private set => moveSpeed = value;}
    [SerializeField] 
    float maxHealth;
    public float MaxHealth {get => maxHealth; private set => maxHealth = value;}
    [SerializeField] 
    float damage;
    public float Damage {get => damage; private set => damage = value;}
}
