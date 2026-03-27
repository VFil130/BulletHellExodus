using System.Collections;
using UnityEngine;

public class FallingProjectiles : ProjectileCast
{
    [SerializeField] private float spawnHeight = 5f;
    [SerializeField] private float spawnAreaWidth = 8f;
    [SerializeField] private bool spawnOneByOne = true;
    [SerializeField] private float spawnDelayBetween = 0.1f;

    public override bool CanUseAbility()
    {
        return true;
    }

    public override void UseAbility()
    {
        if (spawnOneByOne)
        {
            StartCoroutine(ShootMultipleTimes());
        }
        else
        {
            for (int i = 0; i < shoots; i++)
            {
                SetProjectilesTransform();
            }
        }
    }

    protected override IEnumerator ShootMultipleTimes()
    {
        for (int i = 0; i < shoots; i++)
        {
            SetProjectilesTransform();
            if (i < shoots - 1)
            {
                yield return new WaitForSeconds(spawnDelayBetween);
            }
        }
    }

    protected override void SetProjectilesTransform()
    {
        float randomX = Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f) + abilityOwner.transform.position.x;
        float spawnY = abilityOwner.transform.position.y + spawnHeight;

        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);
        Quaternion spawnRotation = Quaternion.identity;

        CreateProjectile(spawnPosition, spawnRotation);
    }
}