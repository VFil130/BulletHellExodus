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

    private List<IAbility> iAbilities = new List<IAbility>(slotsCount);

    private List<float> timers = new List<float>(slotsCount);

    [SerializeField] private int[] abilityLevels = new int[slotsCount];
    [SerializeField] private int abilityCount=0;
    private AbilityUi abilityUi;
    void Awake()
    {
        abilityUi = FindFirstObjectByType<AbilityUi>();
    }
    void FixedUpdate()
    {
        for (int i = 0; i < iAbilities.Count; i++)
        {
            if (iAbilities[i] != null)
            {
                timers[i] += Time.deltaTime;
                abilityUi.CDUpdate(i, timers[i], abilities[i].AbilityInterval);

                if (timers[i] > iAbilities[i].AbilityInterval && iAbilities[i].CanUseAbility())
                {
                    iAbilities[i].UseAbility();
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
            iAbilities.Add(iAbility);
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
