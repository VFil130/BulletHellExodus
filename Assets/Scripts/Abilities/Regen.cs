using UnityEngine;

public class Regen : AbilityStats
{
    private float healAmount = 1;
    public override bool CanUseAbility()
    {
        return true;
    }
    public override void UseAbility()
    {
        abilityOwner.Heal(healAmount);
    }
}
