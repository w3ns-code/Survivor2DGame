using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeWeapon : ProjectileWeapon
{
    public float chainLength = 3f;
    public float repulsionForce = 50f;
    public float attractionForce = -20f;

    protected override void OnProjectileSpawned(Projectile projectile)
    {
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        Rigidbody2D playerRb = owner.GetComponent<Rigidbody2D>();

        if (projectileRb != null && playerRb != null)
        {
            //Le joint
            DistanceJoint2D joint = projectile.gameObject.AddComponent<DistanceJoint2D>();
            joint.connectedBody = playerRb;
            joint.autoConfigureDistance = false;
            joint.distance = chainLength;
            joint.enableCollision = false;

            //Le premier effector comme PointEffector2D
            GameObject pointObj = new GameObject("PointEffector");
            pointObj.transform.SetParent(projectile.transform);
            pointObj.transform.localPosition = Vector3.zero;

            CircleCollider2D pointCollider = pointObj.AddComponent<CircleCollider2D>();
            CircleCollider2D mainCollider = projectile.GetComponent<CircleCollider2D>();
            if (mainCollider != null) pointCollider.radius = mainCollider.radius;
            pointCollider.usedByEffector = true;
            pointCollider.isTrigger = true;

            PointEffector2D pointEffector = pointObj.AddComponent<PointEffector2D>();
            pointEffector.forceMagnitude = repulsionForce;
            pointEffector.forceTarget = EffectorSelection2D.Rigidbody;

            //Le deuxieme effector comme AreaEffector2D
            GameObject areaObj = new GameObject("AreaEffector");
            areaObj.transform.SetParent(projectile.transform);
            areaObj.transform.localPosition = Vector3.zero;

            CircleCollider2D areaCollider = areaObj.AddComponent<CircleCollider2D>();
            if (mainCollider != null) areaCollider.radius = mainCollider.radius * 1.5f;
            areaCollider.usedByEffector = true;
            areaCollider.isTrigger = true;

            AreaEffector2D areaEffector = areaObj.AddComponent<AreaEffector2D>();
            areaEffector.forceMagnitude = attractionForce;
            areaEffector.forceTarget = EffectorSelection2D.Rigidbody;
        }
    }

    protected override float GetSpawnAngle()
    {
        int offset = currentAttackCount > 0 ? currentStats.number - currentAttackCount : 0;
        return 90f - Mathf.Sign(movement.lastMovedVector.x) * (5 * offset);
    }

    protected override Vector2 GetSpawnOffset(float spawnAngle = 0)
    {
        return new Vector2(
            Random.Range(currentStats.spawnVariance.xMin, currentStats.spawnVariance.xMax),
            Random.Range(currentStats.spawnVariance.yMin, currentStats.spawnVariance.yMax)
        );
    }

}