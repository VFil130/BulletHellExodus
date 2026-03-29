using UnityEngine;

public class FireballProjectile : Projectile
{
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private string explosionParticleName = "Explosion";

    protected override void ProjectileEffect(Enemy enemy)
    {
        enemy.TakeMageDamage(currentDamage);
        Explode();
    }

    private void Explode()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, currentRadius, enemyLayerMask);

        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeMageDamage(currentDamage);
                }
            }
        }
        ParticleManager.CreateParticle(explosionParticleName, transform.position);
    }
}