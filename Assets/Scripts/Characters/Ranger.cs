using UnityEngine;

public class Ranger : PlayerController
{
    public override void buffAbility(AbilityStats ability)
    {
        base.buffAbility(ability);
        ability.shoots += 1;
        ability.pierce += 1;
    }
}
