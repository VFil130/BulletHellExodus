using System.Collections.Generic;
using UnityEngine;

public class TakenResources : MonoBehaviour
{
    private Dictionary<Resources, float> inventory = new Dictionary<Resources, float>();
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("ResZone")) 
        {
            ResourceZone resourceZone = collision.GetComponent<ResourceZone>();
            resourceZone.Capturing();
        }
    }
    public void TakeResources(Resources resourceType, float amount)
    {
        if (inventory.ContainsKey(resourceType))
        {
            inventory[resourceType] += amount;
        }
        else
        {
            inventory.Add(resourceType, amount);
        }
        Debug.Log("Получено " + amount + " " + resourceType);
    }
}
