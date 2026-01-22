using UnityEngine;

public abstract class AreaEffect : MonoBehaviour
{
    protected float currentDamage;
    protected float currentDuration;
    protected float currentTickRate;
    [SerializeField] public bool destroy = false;
    [SerializeField] private float lifeTime = 0f;
    private float radius = 3f;

    private float tickTimer = 0f;
    public void Initialize(AbilityStats stats)
    {
        lifeTime = 0;
        currentDamage = stats.damage;
        currentDuration = stats.duration;
        currentTickRate = stats.tickRate;
        radius = stats.radius;
        transform.localScale = Vector3.one * radius * 2;
        tickTimer = currentTickRate;
    }

    void Update()
    {
        lifeTime += Time.deltaTime;
        tickTimer += Time.deltaTime;

        if (tickTimer >= currentTickRate)
        {
            ApplyAreaEffect();
            tickTimer = 0f;
        }

        if (lifeTime >= currentDuration)
        {
            destroy = true;
        }

        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    private void ApplyAreaEffect()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        if (hitColliders.Length > 0)
        {
            foreach (Collider2D collider in hitColliders)
            {
                DoEffect(collider);
            }
        }
    }
    public abstract void DoEffect(Collider2D col);
    protected void MDamage(Enemy enemy)
    {
        enemy.TakeMageDamage(currentDamage);
    }
    protected void PDamage(Enemy enemy)
    {
        enemy.TakePhysDamage(currentDamage);
    }
    protected void CorosionApply(Enemy enemy)
    {
        enemy.CorosionEffect(currentDamage);
    }
}
