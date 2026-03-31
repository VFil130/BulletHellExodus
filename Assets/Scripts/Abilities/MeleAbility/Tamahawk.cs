using UnityEngine;

public class Tamahawk : RotateAroundPlayer
{
    [SerializeField]private float creetChance;
    protected override void Punch(Enemy enemy)
    {
        enemy.TakePhysDamage(CritHit(currentDamage, creetChance));
    }
}
