using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxhealth;
    [SerializeField] private float health;
    [SerializeField] protected float speed;
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
    public int experienceCapIncerease;

    public SpriteRenderer sprite;


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
        if(experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;
            experienceCap += experienceCapIncerease;

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
