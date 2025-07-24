using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityStats : MonoBehaviour, IAbility
{
    [SerializeField] public AbilityData abilityData;
    [SerializeField] private int level;
    [SerializeField] public int Level { get => level; private set => level = value;}
    public Sprite icon;
    public string name;
    [SerializeField] private int id;
    public int ID { get => id; private set => id = value; }

    [SerializeField] private Dictionary<Resources, float> price;
    [SerializeField] private bool isBought;
    public bool ISBought { get => isBought; private set => isBought = value; }
    [SerializeField] public float AbilityInterval { get; set; } = 5f;
    void Awake()
    {
        AbilityInterval = SetCastInterval();
    }
    public void LevelUpAbiliy()
    {
        level++;
        abilityData = abilityData.NextLevelData;
        AbilityInterval = SetCastInterval();
    }
    public void SetLevel1()
    {
        level = 1;
        Level = 1;
    }
    private float SetCastInterval()
    {
        return abilityData.CastInterval;
    }
    public virtual void UseAbility()
    {
        Debug.Log("Cast ability" + name);
    }
    public virtual bool CanUseAbility()
    {
        return true;
    }
    public void Buy(Dictionary<Resources, float> playerResources)
    {
        foreach (var cost in price)
        {
            if (!playerResources.ContainsKey(cost.Key) || playerResources[cost.Key] < cost.Value)
            {
                Debug.Log($"Недостаточно ресурса: {cost.Key} (нужно {cost.Value})");
                return;
            }
        }
        foreach (var cost in price)
        {
            playerResources[cost.Key] -= cost.Value;
        }
        isBought = true;
        Debug.Log("Способность куплена!");
    }
}
