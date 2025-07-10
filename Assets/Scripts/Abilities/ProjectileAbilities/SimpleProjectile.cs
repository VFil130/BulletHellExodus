using UnityEngine;

public class SimpleProjectile : ProjectileCast
{
    public override bool CanUseAbility()
    {
        return true;
    }
    public override void UseAbility()
    {
        StartCoroutine(ShootMultipleTimes());
    }
}
