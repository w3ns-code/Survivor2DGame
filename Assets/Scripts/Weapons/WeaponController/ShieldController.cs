using UnityEngine;

public class ShieldController : WeaponController
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedShield = Instantiate(weaponData.Prefab);
        spawnedShield.transform.position = transform.position; //Meme position que son parent, le player
        spawnedShield.transform.parent = transform; //Pour etre en bas de l'objet
    }
}
