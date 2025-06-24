using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    public AbilityCaster abilityCaster;
    public AbilityStats startAbility;
    [SerializeField] private List<AbilityStats> allAbilities = new List<AbilityStats>();
    [SerializeField] private List<AbilityStats> pickedAbilities;
    [SerializeField] private int numberOfAbilitiesToPick = 4;
    [SerializeField] private List<AbilityStats> AbilitiesToChoose;
    [SerializeField] private int[] abilityLevels;
    [SerializeField] private AbilityCard[] abilityCards = new AbilityCard[4];

    [System.Serializable]
    class AbilityCard
    {
        public TMP_Text name;
        public TMP_Text description;
        public Image icon;
        public GameObject abilityPanel;
    }

    private void Awake()
    {
        if (abilityCaster == null)
        {
            abilityCaster = GetComponent<AbilityCaster>();
        }
    }
    private void Start()
    {
        SpawnFirstAbility();
        TakeRandomAbilities();
    }

    public void PickAbility(AbilityStats ability)
    {
        pickedAbilities = abilityCaster.ReturnAbilities();
        if (!pickedAbilities.Contains(ability))
        {
            abilityCaster.SpawnAbility(ability);
        }
        allAbilities.Remove(ability);
    }
    public void TakeRandomAbilities()
    {
        List<AbilityStats> combinedAbilities = allAbilities.Concat(pickedAbilities).ToList();

        AbilitiesToChoose.Clear();

        while (AbilitiesToChoose.Count < numberOfAbilitiesToPick && combinedAbilities.Count > 0)
        {
            int randomIndex = Random.Range(0, combinedAbilities.Count);
            AbilityStats selectedAbility = combinedAbilities[randomIndex];

            if (!AbilitiesToChoose.Contains(selectedAbility))
            {
                AbilitiesToChoose.Add(selectedAbility);
            }

            combinedAbilities.RemoveAt(randomIndex);
        }
        ShowAbilities();
    }
    private void SpawnFirstAbility()
    {
        abilityCaster.SpawnAbility(startAbility);
        pickedAbilities = abilityCaster.ReturnAbilities();
    }
    public void ChooseAbility (int index)
    {
        if (AbilitiesToChoose[index].Level == 0)
        {
            PickAbility(AbilitiesToChoose[index]);
        }
        else if (AbilitiesToChoose[index].Level > 0)
        {
            AbilitiesToChoose[index].LevelUpAbiliy();
        }
        TakeRandomAbilities();
    }

    public void ShowAbilities()
    {
        for (int i = 0; i < AbilitiesToChoose.Count; i++)
        {
            abilityCards[i].name.text = AbilitiesToChoose[i].name;
            abilityCards[i].icon.sprite = AbilitiesToChoose[i].icon;
            if (AbilitiesToChoose[i].Level > 0)
            {
                abilityCards[i].description.text = AbilitiesToChoose[i].abilityData.NextLevelData.Description;
            }
            else
            {
                abilityCards[i].description.text = AbilitiesToChoose[i].abilityData.Description;
            }
        }
    }
}
