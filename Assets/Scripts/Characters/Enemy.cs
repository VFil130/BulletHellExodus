using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    [SerializeField] private float currentHealth;
    private float currentMoveSpeed;
    private float currentDamage;
    private float currentMageArmour;
    private float currentPhysArmour;
    [SerializeField] public bool IsDead { get; private set; }

    private void Awake()
    {
        currentHealth = enemyData.MaxHealth;
        currentMoveSpeed = enemyData.MoveSpeed;
        currentDamage = enemyData.Damage;
    }
    private void Update()
    {
        Die();
    }
    public void TakePhysDamage(float damage)
    {
        float totalDmg = damage - currentPhysArmour;
        if (totalDmg < 0) { totalDmg = 0; }
        currentHealth -= totalDmg;
    }
    public void TakeMageDamage(float damage)
    {
        float totalDmg = damage - currentMageArmour;
        if (totalDmg < 0) { totalDmg = 0; }
        currentHealth -= totalDmg;
    }
    public void Die()
    {
        if (currentHealth <= 0)
        {
            IsDead = true;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("есть");
            Character character = collision.gameObject.GetComponent<Character>();
            character.TakeDamage(currentDamage,0);
        }
    }
}