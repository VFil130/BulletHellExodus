using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainInventory : MonoBehaviour
{
    private Dictionary<ValResources, float> inventory = new Dictionary<ValResources, float>();
    public static MainInventory instance { get; private set; }


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
    public void Start()
    {
        LoadResources();
    }
    public void SaveResources()
    {
        foreach (var resource in inventory)
        {
            PlayerPrefs.SetFloat($"Resource_{(int)resource.Key}", resource.Value);
        }
        PlayerPrefs.Save();
    }
    public void LoadResources()
    {
        inventory.Clear();
        foreach (ValResources resource in System.Enum.GetValues(typeof(ValResources)))
        {
            float amount = PlayerPrefs.GetFloat($"Resource_{(int)resource}", 0f);
            inventory[resource] = amount;
        }
    }
    public void AddResources(Dictionary<ValResources, float> resourcesToAdd)
    {
        foreach (var resource in resourcesToAdd)
        {
            if (inventory.ContainsKey(resource.Key))
            {
                inventory[resource.Key] += resource.Value;
            }
            else
            {
                inventory[resource.Key] = resource.Value;
            }
        }
    }
    public float GetResourceAmount(ValResources resource)
    {
        if (inventory.ContainsKey(resource))
            return inventory[resource];
        return 0f;
    }
}
