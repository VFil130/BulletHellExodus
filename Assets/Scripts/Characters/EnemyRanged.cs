using UnityEngine;

public class EnemyRanged : Enemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private Transform firePoint;

    private float lastAttackTime;

    public override void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }
    }

    public void Update()
    {

        if (IsDead) return;
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= attackRange)
            {
                TryShoot(player);
            }
        }
    }

    private void TryShoot(GameObject player)
    {
        if (projectilePrefab == null || firePoint == null) return;

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Shoot(player);
            lastAttackTime = Time.time;
        }
    }

    private void Shoot(GameObject player)
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        Vector2 direction = (player.transform.position - firePoint.position).normalized;
        projectile.GetComponent<EnemyProjectile>().SetDirection(direction);
    }
}
