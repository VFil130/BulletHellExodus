using UnityEngine;

public class Wizzard : PlayerController
{
    public override void buffAbility(AbilityStats abiltiy)
    {
        abiltiy.shoots += 1;
        abiltiy.damage *= 1.1f;
    }
}
