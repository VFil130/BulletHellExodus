using UnityEngine;

public class ResourceOrb : MagnetCollectable
{
    [SerializeField] private ValResources resourceType;
    [SerializeField] private float amount = 10f;

    public override void Collect()
    {
        TakenResources.instance?.TakeResources(resourceType, amount);
        Destroy(gameObject);
    }
}