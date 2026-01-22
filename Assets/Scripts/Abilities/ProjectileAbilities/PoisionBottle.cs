using UnityEngine;

public class PoisionBottle : Projectile
{
    [SerializeField]GameObject areaEffect;
   
    protected override void ProjectileEffect(Enemy enemy)
    {
        CreateAreaEffect(enemy.transform.position, areaEffect);
    }
}
