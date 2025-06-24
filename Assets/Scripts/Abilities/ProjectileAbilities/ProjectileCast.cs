using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileCast : AbilityStats
{
    [SerializeField] protected int numberOfShoots = 1;
    [SerializeField] protected float shootInterval = 0.1f;
    [SerializeField] protected GameObject projectilePrefab;
    protected AbilityCaster abilityCaster;
    protected List<GameObject> activeProjectiles = new List<GameObject>();
    private Vector3 projectilePosition;
    private Quaternion projectileRotation;

    void Start()
    {
        abilityCaster = GetComponentInParent<AbilityCaster>();
    }
    protected virtual void CreateProjectile(Vector3 position, Quaternion rotation)
    {
        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, position, rotation);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.Initialize(abilityData);
            activeProjectiles.Add(projectile);
        }
        else
        {
            Debug.LogWarning("снаряд не назначен");
        }
    }
    protected virtual IEnumerator ShootMultipleTimes()
    {
        for (int i = 0; i < numberOfShoots; i++)
        {
            SetProjectilesTransform();
            if (i < numberOfShoots - 1)
            {
                yield return new WaitForSeconds(shootInterval);
            }
        }
    }
    void Update()
    {
        for (int i = activeProjectiles.Count - 1; i >= 0; i--)
        {
            GameObject projectile = activeProjectiles[i];
            if (projectile == null)
            {
                activeProjectiles.RemoveAt(i);
                continue;
            }

            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null && projectileScript.destroy)
            {
                Destroy(projectile);
                activeProjectiles.RemoveAt(i);
            }
        }
    }
    public float SetCastInterval()
    {
        return abilityData.CastInterval;
    }
    protected virtual void SetProjectilesTransform()
    {
        projectilePosition = abilityCaster.transform.position;
        projectileRotation = abilityCaster.transform.rotation;
        CreateProjectile(projectilePosition, projectileRotation);
    }
}
