using System.Collections.Generic;
using UnityEngine;

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
}
