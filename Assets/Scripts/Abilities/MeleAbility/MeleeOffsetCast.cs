using UnityEngine;

public class MeleOffsetCast : MeleCast
{
    [SerializeField]private float x = 0;
    [SerializeField]private float y = 0;
    public override void UseAbility()
    {
        CreateMeleInLocalPosition(x,y);
    }
    public override bool CanUseAbility()
    {
        return true;
    }
}
