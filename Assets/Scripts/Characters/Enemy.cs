using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;
using UnityEngine.Rendering;
public class Enemy : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    public GameObject player;
    [SerializeField] private Color damageColor; 
    [SerializeField] private float currentHealth;
    [SerializeField] private DropManager DM;
    public float currentMoveSpeed;
    private float currentDamage;
    private float currentMageArmour;
    private float currentPhysArmour;
    private Dictionary<int, Coroutine> activeEffects;
    [SerializeField] public bool IsDead { get; private set; }
    [SerializeField]private bool tmpEnemy = false;
    [SerializeField]private float enemyLifeTime;
    private float timer = 0;
    private void Awake()
    {
        currentHealth = enemyData.MaxHealth;
        currentMoveSpeed = enemyData.MoveSpeed;
        currentDamage = enemyData.Damage;
        activeEffects = new Dictionary<int, Coroutine>();
    }
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public virtual void Update()
    {
        if (tmpEnemy == true)
        {
            timer += Time.deltaTime;
            if (timer >= enemyLifeTime)
            {
                DieEffect();
            }
        }
    }
    public virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("есть");
            Character character = collision.gameObject.GetComponent<Character>();
            character.TakeDamage(currentDamage, 0);
        }
    }
    public void UpStatsByWave(float healthMult, float armourMult)
    {
        currentHealth *= healthMult;
        currentMageArmour *= armourMult;
        currentPhysArmour *= armourMult;
    }
    #region Takeingdamage
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
    public void TakeClearDamage(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        EnemyManager.Instance.TriggerDamage(
            transform.position,
            currentDamage,
            EnemyManager.DamageType.Physical
            );
        TakeDamageLogick();
    }
    public void TakeDamageLogick()
    {
        DamageFeedBack();
        Die();
    }
    public void DamageFeedBack()
    {
        Vector3 originalScale = transform.localScale;

        transform.DOKill();
        GetComponent<SpriteRenderer>().DOKill();

        Sequence damageSequence = DOTween.Sequence();

        damageSequence.Append(transform.DOScale(originalScale * 1.1f, 0.1f).SetEase(Ease.OutQuad));
        damageSequence.Join(GetComponent<SpriteRenderer>().DOColor(damageColor, 0.1f));
        damageSequence.AppendInterval(0.1f);
        damageSequence.Append(transform.DOScale(originalScale, 0.1f).SetEase(Ease.InQuad));
        damageSequence.Join(GetComponent<SpriteRenderer>().DOColor(Color.white, 0.1f));
    }
    public void Die()
    {
        if (currentHealth <= 0)
        {
            DieEffect();
            IsDead = true;
        }
        else
        {
            return;
        }
    }
    public void DieNoEffect()
    {
        DM.DropNo();
        IsDead = true;
    }
    public virtual void DieEffect()
    {

    }
    #endregion
    #region EnemyEffects
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
    public void CorosionEffect(float corosionPower)
    {
        if (currentMageArmour <= 0)
        {
            return;
        }
        else
        {
            currentPhysArmour -= corosionPower;
        }
    }
    public void ApplySlow(float power, float time, int id)
    {
        if (activeEffects.ContainsKey(id)) 
        {
            StopCoroutine(activeEffects[id]);
            activeEffects.Remove(id);
            currentMoveSpeed /=power ; // при повышении способности может возникнуть проблема с возвратом неверного значения
            activeEffects.Add(id, StartCoroutine(SlowEffect(power, time)));
        }
        else
        {
            activeEffects.Add(id, StartCoroutine(SlowEffect(power, time)));
        }
    }
    public IEnumerator SlowEffect(float power, float time)
    {
        currentMoveSpeed *= power;
        yield return new WaitForSeconds(time);
        currentMoveSpeed /= power ;
    }
    public void ApplyWeaknes(float power, float time, int id)
    {
        if (activeEffects.ContainsKey(id))
        {
            StopCoroutine(activeEffects[id]);
            activeEffects.Remove(id);
            currentDamage /= power; // при повышении способности может возникнуть проблема с возвратом неверного значения
            activeEffects.Add(id, StartCoroutine(WeaknesEffect(power, time)));
        }
        else
        {
            activeEffects.Add(id, StartCoroutine(WeaknesEffect(power, time)));
        }
    }
    public IEnumerator WeaknesEffect(float power, float time)
    {
        currentDamage *= power;
        yield return new WaitForSeconds(time);
        currentDamage /= power;
    }
    public void ApplyDisArmour(float power, float time, int id, int type)
    {
        if (activeEffects.ContainsKey(id))
        {
            StopCoroutine(activeEffects[id]);
            activeEffects.Remove(id);
            RestoreArmour(type, power);
            activeEffects.Add(id, StartCoroutine(DisArmourEffect(power, time, type)));
        }
        else
        {
            activeEffects.Add(id, StartCoroutine(DisArmourEffect(power, time, type)));
        }
    }

    public IEnumerator DisArmourEffect(float power, float time, int type)
    {
        ApplyArmourReduction(type, power);
        yield return new WaitForSeconds(time);
        RestoreArmour(type, power);
    }

    private void ApplyArmourReduction(int type, float power)
    {
        switch (type)
        {
            case 0:
                currentPhysArmour *= power;
                break;
            case 1:
                currentMageArmour *= power;
                break;
            case 2:
                currentPhysArmour *= power;
                currentMageArmour *= power;
                break;
        }
    }

    private void RestoreArmour(int type, float power)
    {
        switch (type)
        {
            case 0:
                currentPhysArmour /= power;
                break;
            case 1:
                currentMageArmour /= power;
                break;
            case 2:
                currentPhysArmour /= power;
                currentMageArmour /= power;
                break;
        }
    }
    public void ApplyPoison(float damage, float interval, float duration, int id)
    {
        if (activeEffects.ContainsKey(id))
        {
            StopCoroutine(activeEffects[id]);
            activeEffects.Remove(id);
        }
        activeEffects.Add(id, StartCoroutine(PoisonEffect(damage, interval, duration)));
    }

    public IEnumerator PoisonEffect(float damage, float interval, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            currentHealth -= damage;
            timer += interval;
            yield return new WaitForSeconds(interval);
        }
    }
    public virtual void Explode(float radius, float damage)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D target in targets)
        {
            if (target.CompareTag("Player"))
            {
                Character character = target.GetComponent<Character>();
                if (character != null)
                {
                    character.TakeDamage(damage, 0);
                }
            }
            else if (target.CompareTag("Enemy"))
            {
                Enemy enemy = target.GetComponent<Enemy>();
                if (enemy != null && enemy != this)
                {
                    enemy.TakeClearDamage(damage);
                }
            }
        }
    }
    #endregion
}
