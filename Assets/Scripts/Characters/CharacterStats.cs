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
    [SerializeField] private float physArmour;
    [SerializeField] private float mageArmour;
    [SerializeField] private string charInfo;
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
    private float baseSpeed = 1;
    private float ampDamage = 1;
    private float ampSpeed = 1;
    private float ampRadius = 1;
    private float ampDuration = 1;
    private float ampInterval = 1;
    private float ampPower = 1;
    private float ampPierce = 0;
    private float ampShoot = 0;
    private float ampTickRate = 1;
    private float ampMoveSpeed = 1;
    private float ampHealthRegen = 0;
    void Awake()
    {
        baseSpeed = speed;
        LoadUpgrades();
        ApplyCharacterUpgrades();
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
    public void UpdateStats()
    {
        speed = baseSpeed;
        foreach (ActiveEffect effect in activeEffects)
        {
            ApplyEffect(effect);
        }
    }
    public void AddEffect(string effectName, float duration, float power,
                          bool refreshIfExists = true, int maxStacks = 1)
    {
        ActiveEffect existingEffect = activeEffects.Find(e => e.effectName == effectName);

        if (existingEffect != null)
        {
            if (refreshIfExists)
            {
                existingEffect.Refresh(duration);
                existingEffect.TryStack(power);
            }
            else if (maxStacks > 1)
            {
                existingEffect.TryStack(power);
            }
            return;
        }

        ActiveEffect newEffect = new ActiveEffect(effectName, duration, power, maxStacks);
        activeEffects.Add(newEffect);
        UpdateStats();
    }
    public void ApplyEffect(ActiveEffect effect)
    {
        switch (effect.effectName.ToLower())
        {
            case "zone_slow":
                ApplySlowEffect(effect);
                break;
            case "slow":
                ApplySlowEffect(effect);
                break;
            case "poison":
                StartCoroutine(PoisonEffect(effect));
                break;
            case "healover":
                StartCoroutine(HealOverTimeEffect(effect));
                break;
            case "stun":
                ApplyStunEffect(effect);
                break;
            case "speedboost":
                ApplySpeedBoostEffect(effect);
                break;
            case "armorbuff":
                ApplyArmorBuffEffect(effect);
                break;
        }
    }
    public void UpdateEffects()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            ActiveEffect effect = activeEffects[i];
            effect.duration -= Time.deltaTime;

            if (effect.duration <= 0)
            {
                activeEffects.RemoveAt(i);
                UpdateStats();
            }
        }
    }

    public void ClearAllEffects()
    {
        activeEffects.Clear();
    }

    private void ApplySlowEffect(ActiveEffect effect)
    {
        float slowPercent = Mathf.Min(effect.power * effect.stacks, 80f);
        speed = baseSpeed * (1f - slowPercent / 100f);
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
        float boost = effect.power * effect.stacks;
        speed *= boost;

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
        GameManager.instance.SetWinLose(false);
        GameManager.instance.EndGame();
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
    private void LoadUpgrades()
    {
        if(UpgradeManager.instance == null) return;
        var upgrades = UpgradeManager.instance.GetAllUpgrades();

        foreach (var kvp in upgrades)
        {
            UpgradeData upgrade = kvp.Key;
            int level = kvp.Value;
            float totalValue = upgrade.valuePerLevel * level;

            switch (upgrade.stat)
            {
                case UpgradeStat.Damage:
                    ampDamage += totalValue;
                    break;
                case UpgradeStat.Radius:
                    ampRadius += totalValue;
                    break;
                case UpgradeStat.Duration:
                    ampDuration += totalValue;
                    break;
                case UpgradeStat.Pierce:
                    ampPierce += totalValue;
                    break;
                case UpgradeStat.Shoots:
                    ampShoot += totalValue;
                    break;
                case UpgradeStat.Power:
                    ampPower += totalValue;
                    break;
                case UpgradeStat.TickRate:
                    ampTickRate += totalValue;
                    break;
                case UpgradeStat.Interval:
                    ampInterval += totalValue;
                    break;
                case UpgradeStat.Speed:
                    ampSpeed += totalValue;
                    break;
                case UpgradeStat.MaxHealth:
                    maxhealth += totalValue;
                    break;
                case UpgradeStat.PhysArmour:
                    physArmour += totalValue;
                    break;
                case UpgradeStat.MageArmour:
                    mageArmour += totalValue;
                    break;
                case UpgradeStat.MoveSpeed:
                    ampMoveSpeed += totalValue;
                    break;
                case UpgradeStat.HealthRegen:
                    ampHealthRegen += totalValue;
                    break;
            }
        }
    }
    private void ApplyCharacterUpgrades()
    {
        baseSpeed = baseSpeed * ampMoveSpeed;
    }
    public virtual void buffAbility(AbilityStats ability)
    {
        ability.damage *= ampDamage;
        ability.speed *= ampSpeed;
        ability.radius *= ampRadius;
        ability.duration *= ampDuration;
        ability.AbilityInterval /= ampInterval;
        ability.power *= ampPower;
        ability.pierce += ampPierce;
        ability.shoots += ampShoot;
        ability.tickRate /= ampTickRate;
    }
    public string GetCharacterInfo()
    {
        return $"Здоровье: {maxhealth}\n" +
               $"Скорость: {speed}\n" +
               $"Физическая броня: {physArmour}\n" +
               $"Магическая броня: {mageArmour}\n" +
               $"\n{charInfo}";
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
