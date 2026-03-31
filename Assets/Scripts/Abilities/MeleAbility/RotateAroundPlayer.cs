using UnityEngine;

public class RotateAroundPlayer : Melee
{
    public override void DoMove()
    {
        RotateAction();
    }
    public override float SetSpeed()
    {
        return parentAbility.speed;
    }
}
