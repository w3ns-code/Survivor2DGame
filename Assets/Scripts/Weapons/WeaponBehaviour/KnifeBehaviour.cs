using UnityEngine;

public class KnifeBehaviour : ProjectileWeaponBehaviour
{

    KnifeController kc;
  protected override void Start()
    {
        base.Start();
        kc = FindFirstObjectByType<KnifeController>();
    }


    void Update()
    {
        transform.position += direction * currentSpeed * Time.deltaTime; // Le mouvement du couteau
    }
}
