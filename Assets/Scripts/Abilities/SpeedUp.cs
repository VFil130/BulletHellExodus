using UnityEngine;

public class SpeedUp : AbilityStats
{
    public override bool CanUseAbility()
    {
        return true;
    }
    public override void UseAbility()
    {
        abilityOwner.AddEffect("speedboost", duration, speed, true, 1);
    }
}
