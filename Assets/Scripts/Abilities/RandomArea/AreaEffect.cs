using UnityEngine;

public abstract class AreaEffect : MonoBehaviour
{
    private float currentDamage;
    private float currentDuration;
    private float currentTickRate;
    [SerializeField] public bool destroy = false;
    [SerializeField] private float lifeTime = 0f;
    private float radius = 3f;

    private float tickTimer = 0f;
    public AbilityData abilityData;
    public void Initialize(AbilityData data)
    {
        abilityData = data;
        currentDamage = abilityData.Damage;
        currentDuration = abilityData.Duration;
        currentTickRate = abilityData.TickRate;
        radius = abilityData.Radius;

        // Настраиваем размер зоны визуально
        transform.localScale = Vector3.one * radius * 2;

        // Устанавливаем время жизни равным длительности способности
        tickTimer = currentTickRate;
    }

    void Update()
    {
        lifeTime += Time.deltaTime;
        tickTimer += Time.deltaTime;

        // Проверяем tickRate для нанесения периодического урона
        if (tickTimer >= currentTickRate)
        {
            ApplyAreaEffect();
            tickTimer = 0f;
        }

        // Проверяем время жизни
        if (lifeTime >= currentDuration)
        {
            destroy = true;
        }

        // Уничтожаем если нужно
        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    private void ApplyAreaEffect()
    {
        // Ищем всех врагов в радиусе
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
}
