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
        var russianNames = new Dictionary<ValResources, string>
    {
        { ValResources.Res1, "Железо" },
        { ValResources.Res2, "Красное золото" },
        { ValResources.Res3, "Платина" },
        { ValResources.Res4, "Магслиток" }
    };

        string result = "";
        foreach (var res in Inventory)
        {
            string resourceName = russianNames.ContainsKey(res.Key)
                ? russianNames[res.Key]
                : res.Key.ToString();

            result += $"Ресурс: {resourceName}, Количество: {res.Value}\n";
        }
        return result;
    }
}
