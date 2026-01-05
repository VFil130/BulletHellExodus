using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCast : AbilityStats
{
    [SerializeField] protected int numberOfAreas = 1;
    [SerializeField] protected float areaInterval = 0.1f;
    [SerializeField] protected bool isRandom;
    [SerializeField] protected float areaRadius = 5f;
    [SerializeField] private string particleName;

    protected virtual void CreateArea(Vector3 position)
    {
        if (abilityData.AbilityPrefab != null)
        {
            GameObject area = Instantiate(abilityData.AbilityPrefab, position, Quaternion.identity);
            ParticleManager.CreateParticle(particleName, position);
            AreaEffect areaEffect = area.GetComponent<AreaEffect>();

            if (areaEffect != null)
            {
                areaEffect.Initialize(this);
            }
        }
        else
        {
            Debug.LogWarning("Префаб зоны не назначен");
        }
    }

    protected virtual IEnumerator CreateMultipleAreas()
    {
        for (int i = 0; i < numberOfAreas; i++)
        {
            Vector3 spawnPosition = GetAreaSpawnPosition();
            CreateArea(spawnPosition);

            if (i < numberOfAreas - 1)
            {
                yield return new WaitForSeconds(areaInterval);
            }
        }
    }

    protected virtual Vector3 GetAreaSpawnPosition()
    {
        if (isRandom)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float randomDistance = Random.Range(areaRadius * 0.5f, areaRadius * 1.5f);
            return abilityCaster.transform.position + (Vector3)randomDirection * randomDistance;
        }
        else
        {
            Debug.Log("NAYDEN");
            Enemy closestEnemy = abilityCaster.FindClosestEnemyInRadius(abilityCaster.transform.position,areaRadius);

            if (closestEnemy != null)
            {
                return closestEnemy.transform.position;
            }
            else
            {
                return abilityCaster.transform.position + abilityCaster.transform.forward * areaRadius;
            }
        }
    }
    public override void UseAbility()
    {
        StartCoroutine(CreateMultipleAreas());
    }

    public override bool CanUseAbility()
    {
        if (!isRandom)
        {
            Enemy closestEnemy = abilityCaster.FindClosestEnemyInRadius(abilityCaster.transform.position,areaRadius);
            Debug.Log(closestEnemy == null);
            return closestEnemy != null;
        }

        return true;
    }
}
