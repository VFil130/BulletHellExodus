using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    protected AbilityStats parentAbility;
    public void Initialize(AbilityStats stats)
    {
        maxlifeTime = stats.duration;
        parentAbility = stats;
        currentDamage = stats.damage;
        speed = SetSpeed();
    }
    public virtual float SetSpeed()
    {
        float speed = maxlifeTime / 2 - 0.1f;
        return speed;
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
    protected virtual void FixedUpdate()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= maxlifeTime)
        {
            destroy = true;
        }
        DoMove();
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
    public float CritHit(float damage, float critChance)
    {
        float randomValue = UnityEngine.Random.Range(0f, 100f);

        if (randomValue <= critChance)
        {
            return damage * parentAbility.power;
        }

        return damage;
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
    public IEnumerator PulseScale(float duration)
    {
        transform.DOScale(parentAbility.radius, duration).SetEase(Ease.OutQuad);
        Debug.Log("0");
        yield return new WaitForSeconds(duration);
        Debug.Log("1");
        ClearMarkedEnemies();
        Debug.Log("2");
        transform.DOScale(0.25f, duration).SetEase(Ease.InQuad);
        Debug.Log("3");
    }
    public void Poke()
    {
        transform.DOPunchPosition(Vector2.right * 0.8f, parentAbility.duration, 1, 0.8f)
             .SetRelative(true);
    }
    public virtual void DoMove() { }
    public void RotateAction()
    {
        transform.RotateAround(parentAbility.transform.position, Vector3.forward, speed * Time.deltaTime);
    }
}
