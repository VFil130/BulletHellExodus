using System.Collections.Generic;
using System.Linq;
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

    class AbilityCard
    {
        [SerializeField] private string name;
        [SerializeField] private string description;
        [SerializeField] private Image icon;
        [SerializeField] private GameObject abilityPanel;
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
        ChooseRandomAbilities();
    }

    public void PickAbility(AbilityStats ability)
    {
        pickedAbilities = abilityCaster.ReturnAbilities();
        if (!pickedAbilities.Contains(ability))
        {
            pickedAbilities.Add(ability);
            abilityCaster.SpawnAbility(ability);
        }
        allAbilities.Remove(ability);
    }
    public void ChooseRandomAbilities()
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
    }
    private void SpawnFirstAbility()
    {
        abilityCaster.SpawnAbility(startAbility);
        pickedAbilities = abilityCaster.ReturnAbilities();
    }
    private void ChooseAbility (int index)
    {

    }
}
