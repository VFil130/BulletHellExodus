using UnityEngine;

public class Ranger : PlayerController
{
    public override void buffAbility(AbilityStats ability)
    {
        base.buffAbility(ability);
        ability.pierce += 2;
    }
}
