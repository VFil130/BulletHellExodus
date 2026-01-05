using System.Collections;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    [SerializeField] private Color damageColor; 
    [SerializeField] private float currentHealth;
    public float currentMoveSpeed;
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
        EnemyManager.Instance.TriggerDamage(
           transform.position,
           totalDmg,
           EnemyManager.DamageType.Physical
       );
        TakeDamageLogick();
    }
    public void ApplyEffect(BaseEffect newEffect)
    {
        
    }
    public void TakeMageDamage(float damage)
    {
        float totalDmg = damage - currentMageArmour;
        if (totalDmg < 0) { totalDmg = 0; }
        currentHealth -= totalDmg;
        EnemyManager.Instance.TriggerDamage(
            transform.position,
            totalDmg,
            EnemyManager.DamageType.Magical
        );

        TakeDamageLogick();
    }
    public void TakeDamageLogick()
    {
        StartCoroutine(DamageFeedBack());
    }
    private IEnumerator DamageFeedBack()
    {
        Vector3 prevSize;
        prevSize = transform.localScale; 
        transform.localScale *= 1.1f;
        GetComponent<SpriteRenderer>().color = damageColor;
        yield return new WaitForSeconds(0.2f);
        transform.localScale = prevSize;
        GetComponent<SpriteRenderer>().color = Color.white;
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
    public void EmberEffect(float emberPower)
    {
        if (currentMageArmour <= 0)
        {
            TakeMageDamage(emberPower);
        }
        else
        {
            currentMageArmour -= emberPower;
        }
    }
}