using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;

public class AbilityCaster : MonoBehaviour
{
    [SerializeField] static private int slotsCount = 10;
    [SerializeField]
    private List<AbilityStats> abilities;

    private List<float> timers = new List<float>(slotsCount);
    [SerializeField] private int abilityCount=0;
    private AbilityUi abilityUi;
    void Awake()
    {
        abilityUi = FindFirstObjectByType<AbilityUi>();
    }
    void FixedUpdate()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            if (abilities[i] != null)
            {
                timers[i] += Time.deltaTime;
                abilityUi.CDUpdate(i, timers[i], abilities[i].AbilityInterval);

                if (timers[i] > abilities[i].AbilityInterval && abilities[i].CanUseAbility())
                {
                    abilities[i].UseAbility();
                    timers[i] = 0f;
                }
            }
        }
    }
    public void SpawnAbility(AbilityStats ability)
    {
        if (abilityCount <= slotsCount - 1)
        {
            Debug.Log("Додо1 ");
            AbilityStats spawnedAbility = Instantiate(ability, transform.position, Quaternion.identity);
            Debug.Log("Додо2 ");
            spawnedAbility.transform.SetParent(transform);
            AddAbility(spawnedAbility);
        }
    }
    private void AddAbility(AbilityStats abilityPrefab)
    {
        IAbility AbilityToAdd = abilityPrefab.GetComponent<IAbility>();
        if (AbilityToAdd is IAbility iAbility)
        {
            abilities.Add(abilityPrefab);
            timers.Add(0f);
            abilityUi.ChangeImages(abilityCount, abilityPrefab.icon);
            abilityCount++;
            abilityPrefab.SetLevel1();
            Debug.Log("Добавил " + abilityPrefab.name);
        }
        else
        {
            Debug.LogWarning($"Объект {abilityPrefab.name} не реализует интерфейс IAbility");
        }
    }
    public Enemy FindClosestEnemyInRadius(Vector3 center, float radius)
    {
        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        // Используем OverlapSphere для поиска врагов в радиусе
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null && !enemy.IsDead)
                {
                    float distance = Vector3.Distance(center, enemy.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = enemy;
                    }
                }
            }
        }
        return closestEnemy;
    }
    public void LevelUpAbility(int index)
    {
        AbilityStats abilityToUp = abilities[index].GetComponent<AbilityStats>();
        abilityToUp.LevelUpAbiliy();
    }
    public List<AbilityStats> ReturnAbilities()
    {
        return abilities;
    }
}
