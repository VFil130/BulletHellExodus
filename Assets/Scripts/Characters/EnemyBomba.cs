using UnityEngine;

public class EnemyBomba : Enemy
{
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float explosionDamage = 15f;
    public override void DieEffect()
    {
        Explode(explosionRadius, explosionDamage);
        base.DieEffect();
    }

    public override void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Die();
        }
    }
}
