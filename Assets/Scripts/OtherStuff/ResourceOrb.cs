using UnityEngine;

public class ResourceOrb : MagnetCollectable
{
    [SerializeField] private ValResources resourceType;
    [SerializeField] private float amount = 10f;

    public override void Collect()
    {
        MainInventory.instance?.AddResource(resourceType, amount);
        Destroy(gameObject);
    }
}