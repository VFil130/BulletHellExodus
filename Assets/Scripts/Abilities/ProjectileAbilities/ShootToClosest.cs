using System.Collections;
using TMPro;
using UnityEngine;

public class ShootToClosest : ProjectileCast
{
    private Transform closestEnemy;
    public override bool CanUseAbility()
    {
        closestEnemy = abilityCaster.FindClosestEnemyInRadius(transform.position,10f).transform;
        if (closestEnemy == null) return false;
        else return true;
    }

    public override void UseAbility()
    {
        StartCoroutine(ShootMultipleTimes());
    }
    protected override void SetProjectilesTransform()
    {
        closestEnemy = abilityCaster.FindClosestEnemyInRadius(transform.position, 10f).transform;
        projectilePosition = abilityOwner.transform.position;
        RotateTowardsTarget();
        projectileRotation = transform.rotation;
        CreateProjectile(projectilePosition, projectileRotation);
    }
    void RotateTowardsTarget()
    {
        Vector3 direction = closestEnemy.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}