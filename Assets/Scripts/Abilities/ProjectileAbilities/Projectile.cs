using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    [SerializeField] private int id;
    public int ID { get => id; private set => id = value; }
    [SerializeField] protected float currentSpeed;
    [SerializeField] protected float currentDamage;
    [SerializeField] protected float currentPierce;
    [SerializeField] public bool destroy = false;
    [SerializeField] private Vector2 direction;
    [SerializeField] protected float maxlifeTime = 5;
    [SerializeField] protected float lifeTime = 0;
    private bool hasHitEnemy = false;
    private AbilityStats statsLink;

    private HashSet<Enemy> hitEnemies = new HashSet<Enemy>();
    [SerializeField] private CustomCollider customCollider;
    [SerializeField] private LayerMask targetLayerMask;
    public void Initialize(AbilityStats stats)
    {
        ID = stats.ID;
        currentDamage = stats.damage;
        currentPierce = stats.pierce;
        currentSpeed = stats.speed;
        statsLink = stats;
    }
    void FixedUpdate()
    {
        CheckCollisions();
        transform.Translate(direction * currentSpeed * Time.fixedDeltaTime);
        lifeTime += Time.deltaTime;
        if (lifeTime >= maxlifeTime)
        {
            destroy = true;
        }
    }
    private void CheckCollisions()
    {
        if (destroy || hasHitEnemy || customCollider == null) return;

        Collider2D[] hits = customCollider.CheckCollisions(targetLayerMask);

        foreach (var hit in hits)
        {
            if (hit == null) continue;

            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if (!hitEnemies.Contains(enemy))
                    {
                        hitEnemies.Add(enemy);
                        ProjectileEffect(enemy);
                        currentPierce -= 1;

                        if (currentPierce <= 0)
                        {
                            hasHitEnemy = true;
                            destroy = true;
                            break;
                        }
                    }
                }
            }
            else if (hit.CompareTag("Obstacle"))
            {
                destroy = true;
                break;
            }
        }
    }
    protected void CreateAreaEffect(Vector3 position, GameObject areaEffectPrefab)
    {
        if (areaEffectPrefab == null)
        {
            Debug.LogError("NO AreaEffect prefab");
            return;
        }

        GameObject areaObj = Instantiate(areaEffectPrefab, position, Quaternion.identity);
        AreaEffect areaEffect = areaObj.GetComponent<AreaEffect>();
        if (areaEffect != null)
        {
            areaEffect.Initialize(statsLink);
        }
    }
    public void SetLifeTimeZero()
    {
        lifeTime = 0;
        hasHitEnemy = false;
    }
    protected virtual void ProjectileEffect(Enemy enemy)
    {
        enemy.TakeMageDamage(currentDamage);
    }
}