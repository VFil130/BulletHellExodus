using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    private Dictionary<UpgradeData, int> upgradeLevels = new Dictionary<UpgradeData, int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public int GetLevel(UpgradeData upgrade)
    {
        return upgradeLevels.ContainsKey(upgrade) ? upgradeLevels[upgrade] : 0;
    }

    public bool CanBuy(UpgradeData upgrade)
    {
        int currentLevel = GetLevel(upgrade);
        if (currentLevel >= upgrade.costs.Count) return false;

        var cost = upgrade.costs[currentLevel];

        foreach (var resource in cost.resources)
        {
            if (MainInventory.instance.GetResourceAmount(resource.resourceType) < resource.amount)
                return false;
        }
        return true;
    }

    public void BuyUpgrade(UpgradeData upgrade)
    {
        if (!CanBuy(upgrade)) return;

        int currentLevel = GetLevel(upgrade);
        var cost = upgrade.costs[currentLevel];

        var resourcesToSpend = new Dictionary<ValResources, float>();
        foreach (var resource in cost.resources)
        {
            resourcesToSpend[resource.resourceType] = -resource.amount;
        }
        MainInventory.instance.AddResources(resourcesToSpend);

        upgradeLevels[upgrade] = currentLevel + 1;
    }
    public Dictionary<UpgradeData, int> GetAllUpgrades()
    {
        return new Dictionary<UpgradeData, int>(upgradeLevels);
    }
}