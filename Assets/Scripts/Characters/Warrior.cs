using UnityEngine;

public class Warrior : PlayerController
{
    public override void buffAbility(AbilityStats abiltiy)
    {
        abiltiy.damage *= 1.5f;
    }
}
