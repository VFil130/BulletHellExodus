using UnityEngine;
using UnityEngine.UI;

public class AbilityStats : MonoBehaviour, IAbility
{
    [SerializeField] public AbilityData abilityData;
    [SerializeField] private int level;
    [SerializeField] public int Level { get => level; private set => level = value;}
    public Sprite icon;
    public string name;
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
}
