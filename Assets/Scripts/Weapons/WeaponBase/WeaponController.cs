using UnityEngine;
using UnityEngine.Timeline;

public class WeaponController : MonoBehaviour
{
    public WeaponScriptableObjects weaponData;
    float currentCooldown;


    protected PlayerMove pm;

  //  Virtual pour un override
  protected virtual void Start()
    {
        pm = FindFirstObjectByType<PlayerMove>();
        currentCooldown = weaponData.CooldownDuration;
    }

    protected virtual void Update()
    {
       currentCooldown -= Time.deltaTime;
       if(currentCooldown <= 0f) // Temps est 0 -> produit un attaque
        {
            Attack();
        } 
    }

    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration;
    }
}
