using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [Header("Active Effects")]
    public List<ActiveEffect> activeEffects = new List<ActiveEffect>();
    [Header("Stats")]
    [SerializeField] private float maxhealth;
    [SerializeField] private float health;
    [SerializeField] protected float speed;
    private float baseSpeed;
    [SerializeField] private float physArmour;
    [SerializeField] private float mageArmour;
    [Header("I-Frames")]
    [SerializeField] private float invincibilityDuration;
    [SerializeField] private bool isInvincible;
    public bool isDead = false;
    [Header("Experience")]
    public float experience = 0;
    public int level = 1;
    public float experienceCap = 100;
    private int experienceCapBase = 100;
    private float experienceGrowthRate = 1.15f;

    public SpriteRenderer sprite;


    private void Start()
    {
        baseSpeed = speed;
    }
    private void Update()
    {
        if (isDead) return;
        UpdateEffects(Time.deltaTime);
    }
    #region Effects System
    [System.Serializable]
    public class ActiveEffect
    {
        public string effectName;
        public float duration;
        public float power;
        public int stacks;
        public float maxDuration;
        public int maxStacks;

        public ActiveEffect(string name, float dur, float pow, int maxStacks = 1)
        {
            effectName = name;
            duration = dur;
            maxDuration = dur;
            power = pow;
            stacks = 1;
            this.maxStacks = maxStacks;
        }

        public void Refresh(float newDuration)
        {
            duration = newDuration;
            maxDuration = Mathf.Max(maxDuration, newDuration);
        }

        public bool TryStack(float addPower)
        {
            if (stacks < maxStacks)
            {
                stacks++;
                power += addPower;
                return true;
            }
            return false;
        }
    }

    public void ApplyEffect(string effectName, float duration, float power,
                          bool refreshIfExists = true, int maxStacks = 1,
                          float stackPowerBonus = 0)
    {
        ActiveEffect existingEffect = activeEffects.Find(e => e.effectName == effectName);

        if (existingEffect != null)
        {
            if (refreshIfExists)
            {
                existingEffect.Refresh(duration);
                existingEffect.TryStack(stackPowerBonus);
            }
            else if (maxStacks > 1)
            {
                existingEffect.TryStack(stackPowerBonus);
            }
            return;
        }

        ActiveEffect newEffect = new ActiveEffect(effectName, duration, power, maxStacks);
        activeEffects.Add(newEffect);

        switch (effectName.ToLower())
        {
            case "slow":
                ApplySlowEffect(newEffect);
                break;
            case "poison":
                StartCoroutine(PoisonEffect(newEffect));
                break;
            case "healover":
                StartCoroutine(HealOverTimeEffect(newEffect));
                break;
            case "stun":
                ApplyStunEffect(newEffect);
                break;
            case "speedboost":
                ApplySpeedBoostEffect(newEffect);
                break;
            case "armorbuff":
                ApplyArmorBuffEffect(newEffect);
                break;
        }
    }

    private void UpdateEffects(float deltaTime)
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            ActiveEffect effect = activeEffects[i];
            effect.duration -= deltaTime;

            if (effect.duration <= 0)
            {
                RemoveEffect(effect.effectName);
                activeEffects.RemoveAt(i);
            }
        }
    }

    public void RemoveEffect(string effectName)
    {
        ActiveEffect effect = activeEffects.Find(e => e.effectName == effectName);
        if (effect != null)
        {
            switch (effectName.ToLower())
            {
                case "slow":
                    speed = baseSpeed;
                    break;
                case "stun":
                    break;
                case "speedboost":
                    speed = baseSpeed;
                    break;
                case "armorbuff":
                    physArmour = 0;
                    mageArmour = 0;
                    break;
            }
        }
    }

    public void ClearAllEffects()
    {
        foreach (ActiveEffect effect in activeEffects)
        {
            RemoveEffect(effect.effectName);
        }
        activeEffects.Clear();
    }

    private void ApplySlowEffect(ActiveEffect effect)
    {
        float slowPercent = Mathf.Min(effect.power * effect.stacks, 80f);
        speed = baseSpeed * (1f - slowPercent / 100f);

        if (sprite != null)
            StartCoroutine(FlashColor(Color.blue, 0.5f));
    }

    private IEnumerator PoisonEffect(ActiveEffect effect)
    {
        float tickRate = 1f;

        while (effect.duration > 0 && HasEffect("poison"))
        {
            TakeDamage(effect.power * effect.stacks, 0);
            if (sprite != null)
                StartCoroutine(FlashColor(Color.green, 0.3f));

            yield return new WaitForSeconds(tickRate);
        }
    }

    private IEnumerator HealOverTimeEffect(ActiveEffect effect)
    {
        float tickRate = 1f;

        while (effect.duration > 0 && HasEffect("healover"))
        {
            Heal(effect.power * effect.stacks);
            if (sprite != null)
                StartCoroutine(FlashColor(Color.green, 0.3f));

            yield return new WaitForSeconds(tickRate);
        }
    }

    private void ApplyStunEffect(ActiveEffect effect)
    {
        speed = 0;
        if (sprite != null)
            sprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    private void ApplySpeedBoostEffect(ActiveEffect effect)
    {
        float boostPercent = Mathf.Min(effect.power * effect.stacks, 100f);
        speed = baseSpeed * (1f + boostPercent / 100f);

        if (sprite != null)
            StartCoroutine(FlashColor(Color.yellow, 0.5f));
    }

    private void ApplyArmorBuffEffect(ActiveEffect effect)
    {
        physArmour += effect.power * effect.stacks;
        mageArmour += effect.power * effect.stacks;

        if (sprite != null)
            StartCoroutine(FlashColor(Color.white, 0.5f));
    }

    private IEnumerator FlashColor(Color color, float duration)
    {
        Color originalColor = sprite.color;
        sprite.color = color;
        yield return new WaitForSeconds(duration);
        sprite.color = originalColor;
    }

    public bool HasEffect(string effectName)
    {
        return activeEffects.Exists(e => e.effectName == effectName);
    }

    public ActiveEffect GetEffect(string effectName)
    {
        return activeEffects.Find(e => e.effectName == effectName);
    }
    #endregion
    public float TakePhysDamage(float damage)
    {
        float totalDmg = damage - physArmour;
        if (totalDmg < 0) { totalDmg = 0; }
        return totalDmg;
    }
    public float TakeMageDamage(float damage)
    {
        float totalDmg = damage - mageArmour;
        if (totalDmg < 0) { totalDmg = 0; }
        return totalDmg;
    }
    public void TakeDamage(float mdmg, float pdmg)
    {
        if (!isInvincible)
        {
            float totalDmg = TakeMageDamage(mdmg) + TakePhysDamage(pdmg);
            health -= totalDmg;
            isInvincible = true;
            StartCoroutine(Invincibility());
            if (health <= 0)
            {
                Die();
            }
        }
    }
    public void Heal(float heal)
    {
        if (health + heal >= maxhealth)
        {
            health = maxhealth;
        }
        else 
        {
            health += heal;
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    private IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
    public void IncreaseExperience(int amount)
    {
        experience += amount;

        LevelUpChecker();
    }
    void LevelUpChecker()
    {
        while (experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            experienceCap = Mathf.RoundToInt(experienceCapBase * Mathf.Pow(experienceGrowthRate, level - 1) + experienceCapBase*level);

            GameManager.instance.StartLevelUp();
        }
    }
    public virtual void buffAbility(AbilityStats abiltiy)
    {
        Debug.Log(abiltiy.name + "buffed");
    } 
    public float ReturnHealth()
    {
        return health / maxhealth;
    }
    public float ExpToNextLevel()
    {
        return (experience/experienceCap);
    }
}
