using UnityEngine;

public class RotateAroundPlayer : Melee
{
    [SerializeField] private float angle;
    [SerializeField] private float updateTime;
    public override void DoMove()
    {
        RotateAction();
    }
    public override float SetSpeed()
    {
        return parentAbility.speed;
    }
}
