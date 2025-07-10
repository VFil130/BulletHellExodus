using UnityEngine;

public class Slash : MeleCast
{
    public override void UseAbility()
    {
        CreateMele();
    }
    public override bool CanUseAbility()
    {
        return true;
    }
}
