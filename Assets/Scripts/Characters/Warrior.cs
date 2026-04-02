using UnityEngine;

public class Warrior : PlayerController
{
    public override void buffAbility(AbilityStats ability)
    {
        ability.damage *= 1.5f;
    }
}
