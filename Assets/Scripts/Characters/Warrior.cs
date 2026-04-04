using UnityEngine;

public class Warrior : PlayerController
{
    public override void buffAbility(AbilityStats ability)
    {
        base.buffAbility(ability);
        ability.damage *= 1.5f;
    }
}
