using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    [SerializeField] private int id;
    public int ID { get => id; private set => id = value; }
    [SerializeField] private float currentSpeed;
    [SerializeField] private float currentDamage;
    [SerializeField] private float currentPierce;
    [SerializeField] public bool destroy = false;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float maxlifeTime = 5;
    [SerializeField] private float lifeTime = 0;
    private bool hasHitEnemy = false; 


    public void Initialize(AbilityStats stats)
    {
        ID = stats.ID;
        currentDamage = stats.damage;
        currentPierce = stats.pierce;
        currentSpeed = stats.speed;
    }
    void FixedUpdate()
    {
        transform.Translate(direction * currentSpeed * Time.fixedDeltaTime);
        lifeTime += Time.deltaTime;
        if (lifeTime >= maxlifeTime)
        {
            destroy = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasHitEnemy && collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeMageDamage(currentDamage);
            currentPierce -= 1;
            if (currentPierce <= 0) 
            {
                hasHitEnemy = true;
                destroy = true;
            }
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            destroy = true;
        }
    }
    public void SetLifeTimeZero()
    {
        lifeTime = 0;
    }
}