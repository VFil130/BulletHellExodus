using UnityEngine;

public class Ranger : PlayerController
{
    public override void buffAbility(AbilityStats abiltiy)
    {
        abiltiy.shoots += 1;
        abiltiy.pierce += 1;
    }
}
