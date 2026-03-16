using UnityEngine;

/// <summary>
/// Composant que vous attachez à tous les prefabs de projectiles. Tous les projectiles générés voleront dans la direction
/// à laquelle ils font face et infligeront des dégâts lorsqu'ils toucheront un objet.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : WeaponEffect
{

    public enum DamageSource { projectile, owner };
    public DamageSource damageSource = DamageSource.projectile;
    public bool hasAutoAim = false;
    public Vector3 rotationSpeed = new Vector3(0, 0, 0);

    protected Rigidbody2D rb;
    protected int piercing;

    // Start est appelé avant la première mise à jour de frame
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Weapon.Stats stats = weapon.GetStats();
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            rb.angularVelocity = rotationSpeed.z;
            rb.linearVelocity = transform.right * stats.speed * weapon.Owner.Stats.speed;
        }

        // Empêche la zone d'être 0, car cela cache le projectile.
        float area = weapon.GetArea();
        if(area <= 0) area = 1;
        transform.localScale = new Vector3(
            area * Mathf.Sign(transform.localScale.x),
            area * Mathf.Sign(transform.localScale.y), 1
        );

        // Définit le nombre de perçages de cet objet.
        piercing = stats.piercing;

        // Détruit le projectile après l'expiration de sa durée de vie.
        if (stats.lifespan > 0) Destroy(gameObject, stats.lifespan);

        // Si le projectile a une visée automatique, trouve automatiquement un ennemi approprié.
        if (hasAutoAim) AcquireAutoAimFacing();
    }

    // Si le projectile est à tête chercheuse, il trouvera automatiquement une cible appropriée
    // vers laquelle se diriger.
    public virtual void AcquireAutoAimFacing()
    {
        float aimAngle; // Nous devons déterminer où viser.

        // Trouve tous les ennemis à l'écran.
        EnemyStats[] targets = FindObjectsOfType<EnemyStats>();

        // Sélectionne un ennemi au hasard (s'il y en a au moins 1).
        // Sinon, choisit un angle aléatoire.
        if (targets.Length > 0)
        {
            EnemyStats selectedTarget = targets[Random.Range(0, targets.Length)];
            Vector2 difference = selectedTarget.transform.position - transform.position;
            aimAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }
        else
        {
            aimAngle = Random.Range(0f, 360f);
        }

        // Oriente le projectile vers l'endroit où nous visons.
        transform.rotation = Quaternion.Euler(0, 0, aimAngle);
    }

    // FixedUpdate est appelé à intervalle fixe
    protected virtual void FixedUpdate()
    {
        // Ne gère le mouvement nous-mêmes que si c'est un kinematic.
        if (rb.bodyType == RigidbodyType2D.Kinematic)
        {
            Weapon.Stats stats = weapon.GetStats();
            transform.position += transform.right * stats.speed * weapon.Owner.Stats.speed * Time.fixedDeltaTime;
            rb.MovePosition(transform.position);
            transform.Rotate(rotationSpeed * Time.fixedDeltaTime);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        EnemyStats es = other.GetComponent<EnemyStats>();
        BreakableProps p = other.GetComponent<BreakableProps>();

        // Ne collisionne qu'avec les ennemis ou les objets cassables.
        if (es)
        {
            // S'il y a un propriétaire, et que la source de dégâts est définie sur le propriétaire,
            // nous calculerons le recul en utilisant le propriétaire au lieu du projectile.
            Vector3 source = damageSource == DamageSource.owner && owner ? owner.transform.position : transform.position;

            // Inflige des dégâts et détruit le projectile.
            es.TakeDamage(GetDamage(), source);

            // Récupère les statistiques de l'arme.
            Weapon.Stats stats = weapon.GetStats();

            weapon.ApplyBuffs(es); // Applique tous les buffs assignés à la cible.

            // Réduit la valeur de perçage, et détruit le projectile s'il n'a plus de perçage.
            piercing--;
            if (stats.hitEffect)
            {
                Destroy(Instantiate(stats.hitEffect, transform.position, Quaternion.identity), 5f);
            }
        }
        else if (p)
        {
            p.TakeDamage(GetDamage());
            piercing--;

            Weapon.Stats stats = weapon.GetStats();
            if (stats.hitEffect)
            {
                Destroy(Instantiate(stats.hitEffect, transform.position, Quaternion.identity), 5f);
            }
        }

        // Détruit cet objet s'il n'a plus de "vie" après avoir heurté d'autres objets.
        if (piercing <= 0) Destroy(gameObject);
    }
}