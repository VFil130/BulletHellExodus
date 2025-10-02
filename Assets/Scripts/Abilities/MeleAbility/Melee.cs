using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// здесь описанные данные создаваемого префаба для ближних атак
public class Melee : MonoBehaviour
{
    [SerializeField] private float currentDamage;
    [SerializeField] public bool destroy = false;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float maxlifeTime = 0.2f;
    [SerializeField] private float lifeTime = 0;
    public float punchForce = 2f;
    public AbilityData abilityData;
    [SerializeField] private List<GameObject> markedEnemies;
    public void Initialize(AbilityData data)
    {
        abilityData = data;
        currentDamage = abilityData.Damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !markedEnemies.Contains(collision.gameObject))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            markedEnemies.Add(collision.gameObject);
            Punch(enemy);
        }
    }
    void FixedUpdate()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= maxlifeTime)
        {
            destroy = true;
        }
    }
    private void Punch(Enemy enemy)
    {
        enemy.TakePhysDamage(currentDamage);
        PushEnemy(enemy);
    }
    private void PushEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            Vector3 pushDirection;
            pushDirection = enemy.transform.position - transform.position;
            pushDirection.Normalize();
            enemy.transform.Translate(pushDirection*punchForce);
        }
    }
}
