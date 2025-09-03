using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    [SerializeField] private bool isLocked = true;
    [SerializeField] private bool isPicked = true;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private Button slotButton;
    [SerializeField] private AbilityStats ability;

    public void Initialize(AbilityStats abilityStats)
    {
        ability = abilityStats;
        nameText = GetComponentInChildren<TMP_Text>();
        lockIcon = transform.Find("LockIcon")?.gameObject;  
        slotButton = GetComponent<Button>();
        isLocked = !ability.ISBought;
        if (!isLocked)
        {
            Unlock();
        }
        UpdateUI();
    }
    public void Unlock()
    {
        isLocked = false;
        lockIcon.gameObject.SetActive(false);
    }
    private void UpdateUI()
    {
        nameText.text = ability.name;
        iconImage.sprite = ability.icon;
        lockIcon.SetActive(isLocked);
        slotButton.interactable = !isLocked;
    }
}
