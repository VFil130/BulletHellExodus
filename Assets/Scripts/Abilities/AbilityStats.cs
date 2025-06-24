using UnityEngine;
using UnityEngine.UI;

public class AbilityStats : MonoBehaviour
{
    [SerializeField] protected AbilityData abilityData;
    [SerializeField] private int id;
    [SerializeField] public int Id { get => id; private set => id = value;}
    public Sprite icon;
    public void LevelUpAbiliy()
    {
        abilityData = abilityData.NextLevelData;
    }
}
