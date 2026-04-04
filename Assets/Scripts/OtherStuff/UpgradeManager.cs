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
            LoadUpgrades();
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
        SaveUpgrades();
    }

    public Dictionary<UpgradeData, int> GetAllUpgrades()
    {
        return new Dictionary<UpgradeData, int>(upgradeLevels);
    }

    private void SaveUpgrades()
    {
        foreach (var kvp in upgradeLevels)
        {
            string key = kvp.Key.name;
            PlayerPrefs.SetInt(key, kvp.Value);
        }
        PlayerPrefs.Save();
    }

    private void LoadUpgrades()
    {
        upgradeLevels.Clear();

        UpgradeData[] allUpgrades = Resources.LoadAll<UpgradeData>("");

        foreach (var upgrade in allUpgrades)
        {
            string key = upgrade.name;
            int level = PlayerPrefs.GetInt(key, 0);
            if (level > 0)
            {
                upgradeLevels[upgrade] = level;
            }
        }
    }
}