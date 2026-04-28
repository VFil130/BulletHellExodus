using System.Collections;
using UnityEngine;

public class LichBoss : Enemy
{
    [Header("Boss Settings")]
    [SerializeField] private float passiveAttackInterval = 2f;
    [SerializeField] private float projectileDelay = 0.25f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private Animator animator;
    [Header("Special Attack")]
    [SerializeField] private float specialCooldown = 10f;
    [SerializeField] private float specialChance = 0.5f;
    [SerializeField] private AnimationClip specialAnimation;
    [SerializeField] private int waveCount = 3;
    [SerializeField] private int projectilesPerWave = 12;
    [SerializeField] private float waveSpreadAngle = 15f;
    [SerializeField] private float specialProjectileSpeed = 6f;

    private float lastPassiveAttackTime;
    private float lastSpecialCheckTime;
    private bool isCastingSpecial = false;
    private bool useLeftSpawn = true;

    public override void Update()
    {
        base.Update();
        if (IsDead) return;
        if (isCastingSpecial) return;

        HandlePassiveAttacks();
        HandleSpecialAttack();
    }

    private void HandlePassiveAttacks()
    {
        if (Time.time >= lastPassiveAttackTime + passiveAttackInterval)
        {
            StartCoroutine(PassiveAttack());
            lastPassiveAttackTime = Time.time;
        }
    }

    private IEnumerator PassiveAttack()
    {
        if (useLeftSpawn)
        {
            SpawnProjectile(leftSpawnPoint);
            useLeftSpawn = false;
        }
        else
        {
            SpawnProjectile(rightSpawnPoint);
            useLeftSpawn = true;
        }

        yield return new WaitForSeconds(projectileDelay);

        if (useLeftSpawn)
        {
            SpawnProjectile(leftSpawnPoint);
            useLeftSpawn = false;
        }
        else
        {
            SpawnProjectile(rightSpawnPoint);
            useLeftSpawn = true;
        }
    }

    private void SpawnProjectile(Transform spawnPoint)
    {
        if (spawnPoint == null || projectilePrefab == null) return;
        if (player == null) return;

        GameObject proj = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        Vector2 direction = (player.transform.position - spawnPoint.position).normalized;
        proj.GetComponent<EnemyProjectile>().SetDirection(direction);
        proj.GetComponent<EnemyProjectile>().SetSpeed(projectileSpeed);
    }

    private void HandleSpecialAttack()
    {
        if (Time.time >= lastSpecialCheckTime + specialCooldown)
        {
            lastSpecialCheckTime = Time.time;

            if (Random.value <= specialChance)
            {
                StartCoroutine(SpecialAttack());
            }
        }
    }

    private IEnumerator SpecialAttack()
    {
        isCastingSpecial = true;
        currentMoveSpeed = 0;

        if (animator != null)
        {
            animator.Play("LichCast");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * 3);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < waveCount; i++)
        {
            float angleOffset = i * waveSpreadAngle - (waveCount - 1) * waveSpreadAngle / 2f;
            SpawnWave(angleOffset);
            yield return new WaitForSeconds(0.1f);
        }

        currentMoveSpeed = enemyData.MoveSpeed;
        isCastingSpecial = false;
    }

    private void SpawnWave(float angleOffset)
    {
        float angleStep = 360f / projectilesPerWave;

        for (int i = 0; i < projectilesPerWave; i++)
        {
            float angle = i * angleStep + angleOffset;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            EnemyProjectile projectile = proj.GetComponent<EnemyProjectile>();
            projectile.SetDirection(direction);
            projectile.SetSpeed(specialProjectileSpeed);
        }
    }

    public override void DieEffect()
    {
        base.DieEffect();
    }
}