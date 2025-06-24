using UnityEngine;
using UnityEngine.UI;

public class AbilityStats : MonoBehaviour
{
    [SerializeField] public AbilityData abilityData;
    [SerializeField] private int level;
    [SerializeField] public int Level { get => level; private set => level = value;}
    public Sprite icon;
    public string name;
    public void LevelUpAbiliy()
    {
        level++;
        abilityData = abilityData.NextLevelData;
    }
    public void SetLevel1()
    {
        level = 1;
        Level = 1;
    }
}
