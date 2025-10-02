using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    [SerializeField] private float currentHealth;
    private float currentMoveSpeed;
    private float currentDamage;
    private float currentMageArmour;
    private float currentPhysArmour;
    [SerializeField] public bool IsDead { get; private set; }

    //[SerializeField] private Image healthBar;

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
    void UpdateHealthBar() // возможно не надо
    {
        //healthBar.fillAmount = enemyData.MaxHealth / currentHealth;
    }
    public void TakePhysDamage(float damage)
    {
        float totalDmg = damage - currentPhysArmour;
        if (totalDmg < 0) { totalDmg = 0; }
        currentHealth -= totalDmg;
        //UpdateHealthBar();
    }
    public void TakeMageDamage(float damage)
    {
        float totalDmg = damage - currentMageArmour;
        if (totalDmg < 0) { totalDmg = 0; }
        currentHealth -= totalDmg;
        //UpdateHealthBar();
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