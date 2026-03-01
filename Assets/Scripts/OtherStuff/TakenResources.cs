using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class TakenResources : MonoBehaviour
{
    public static TakenResources instance { get; private set; }
    public void Awake()
    {
        instance = this;
    }
    public Dictionary<ValResources, float> Inventory { get; private set; } = new Dictionary<ValResources, float>();
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("ResZone")) 
        {
            ResourceZone resourceZone = collision.GetComponent<ResourceZone>();
            resourceZone.Capturing();
        }
    }
    public void TakeResources(ValResources resourceType, float amount)
    {
        if (Inventory.ContainsKey(resourceType))
        {
            Inventory[resourceType] += amount;
        }
        else
        {
            Inventory.Add(resourceType, amount);
        }
        Debug.Log("Получено " + amount + " " + resourceType);
    }
    public string ReturnCapturedResources()
    {
        string result = "";
        foreach (var res in Inventory)
        {
            result += $"Ресурс: {res.Key}, Количество: {res.Value}\n";
        }
        return result;
    }
}
