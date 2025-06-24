using UnityEngine;

public class SimpleProjectile : ProjectileCast, IAbility
{
    public float AbilityInterval { get; set; } = 20;
    void Awake()
    {
        AbilityInterval = SetCastInterval();
    }
    public bool CanUseAbility()
    {
        return true;
    }
    public void UseAbility()
    {
        StartCoroutine(ShootMultipleTimes());
    }
}
