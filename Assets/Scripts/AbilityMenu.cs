using UnityEngine;

public class AbilityMenu : MonoBehaviour
{
    [SerializeField] private AbilityStats[] allAbilities;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private AbilitySlot[] ChosedAbilities;
    private void Start()
    {
        foreach (var ability in allAbilities)
        {
            GameObject slotObj = Instantiate(slotPrefab, transform);
            AbilitySlot slot = slotObj.GetComponent<AbilitySlot>();
            slot.Initialize(ability);
        }
    }
}
