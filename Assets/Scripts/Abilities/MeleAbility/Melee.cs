using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Melee : MonoBehaviour
{
    public float currentDamage;
    [SerializeField] public bool destroy = false;
    [SerializeField] private float maxlifeTime = 0.2f;
    [SerializeField] private float lifeTime = 0;
    [SerializeField] private float speed;
    public float punchForce = 2f;
    [SerializeField] private List<GameObject> markedEnemies;
    private AbilityStats parentAbility;
    public void Initialize(AbilityStats stats)
    {
        parentAbility = stats;
        currentDamage = stats.damage;
        speed = maxlifeTime/2 - 0.1f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ВОШЛО");
        if (collision.gameObject.CompareTag("Enemy") && !markedEnemies.Contains(collision.gameObject))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            markedEnemies.Add(collision.gameObject);
            Punch(enemy);
        }
    }
    protected virtual void FixedUpdate()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= maxlifeTime)
        {
            destroy = true;
        }
    }
    protected virtual void Punch(Enemy enemy)
    {
        enemy.TakePhysDamage(currentDamage);
        PushEnemy(enemy);
    }
    protected void PushEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            Vector3 pushDirection;
            pushDirection = enemy.transform.position - transform.position;
            pushDirection.Normalize();
            enemy.transform.Translate(pushDirection*punchForce);
        }
    }
    private void ClearMarkedEnemies()
    {
        markedEnemies.Clear();
    }
    public IEnumerator Swinging()
    {
        transform.DORotate(new Vector3(0, 0, 90), speed, RotateMode.LocalAxisAdd);
        yield return new WaitForSeconds(0.05f);
        ClearMarkedEnemies();
        transform.DORotate(new Vector3(0, 0, -180), speed, RotateMode.LocalAxisAdd);
        yield return null;
    }
    public void Poke()
    {
        transform.DOPunchPosition(Vector2.right * 0.8f, parentAbility.duration, 1, 0.8f)
             .SetRelative(true);
    }
}
