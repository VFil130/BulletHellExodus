using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUISlot : MonoBehaviour
{
    [SerializeField] private UpgradeData upgrade;
    [SerializeField] private TextMeshProUGUI effectText;
    [SerializeField] private TextMeshProUGUI costText1;
    [SerializeField] private TextMeshProUGUI costText2;
    [SerializeField] private TextMeshProUGUI costText3;
    [SerializeField] private TextMeshProUGUI costText4;
    [SerializeField] private Button buyButton;

    void Start()
    {
        if (upgrade != null)
        {
            UpdateUI();
            buyButton.onClick.AddListener(OnBuyClick);
        }
    }

    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        int level = UpgradeManager.instance.GetLevel(upgrade);
        float nextValue = upgrade.valuePerLevel * (level + 1);

        if (upgrade.valuePerLevel < 1 && upgrade.valuePerLevel > 0)
        {
            effectText.text = $"+{nextValue * 100:F0}%";
        }
        else
        {
            effectText.text = $"+{nextValue:F1}";
        }

        if (level >= upgrade.costs.Count)
        {
            costText1.text = "MAX";
            costText2.gameObject.SetActive(false);
            costText3.gameObject.SetActive(false);
            costText4.gameObject.SetActive(false);
            buyButton.interactable = false;
            return;
        }

        costText1.gameObject.SetActive(true);
        costText2.gameObject.SetActive(true);
        costText3.gameObject.SetActive(true);
        costText4.gameObject.SetActive(true);

        var cost = upgrade.costs[level];

        TextMeshProUGUI[] costTexts = { costText1, costText2, costText3, costText4 };

        for (int i = 0; i < costTexts.Length; i++)
        {
            if (i < cost.resources.Count)
            {
                var res = cost.resources[i];
                float currentAmount = MainInventory.instance.GetResourceAmount(res.resourceType);
                costTexts[i].text = $"{res.amount}";

                if (currentAmount < res.amount)
                    costTexts[i].color = Color.red;
                else
                    costTexts[i].color = Color.white;
            }
            else
            {
                costTexts[i].text = "0";
                costTexts[i].color = Color.white;
            }
        }

        buyButton.interactable = UpgradeManager.instance.CanBuy(upgrade);
    }

    private void OnBuyClick()
    {
        UpgradeManager.instance.BuyUpgrade(upgrade);
        UpdateUI();
    }
}