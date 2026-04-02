using UnityEngine;

public class Ranger : PlayerController
{
    public override void buffAbility(AbilityStats ability)
    {
        ability.shoots += 1;
        ability.pierce += 1;
    }
}
