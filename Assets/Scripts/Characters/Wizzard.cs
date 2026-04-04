using UnityEngine;

public class Wizzard : PlayerController
{
    public override void buffAbility(AbilityStats ability)
    {
        base.buffAbility(ability);
        ability.shoots += 1;
        ability.damage *= 1.1f;
    }
}
