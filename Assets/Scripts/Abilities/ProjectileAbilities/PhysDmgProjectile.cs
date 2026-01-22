using UnityEngine;

public class PhysDmgProjectile : Projectile
{
    protected override void ProjectileEffect(Enemy enemy)
    {
        enemy.TakePhysDamage(currentDamage);
    }
}
