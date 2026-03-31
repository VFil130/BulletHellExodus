using UnityEngine;

public class Tornado : AreaEffect
{
    public override void DoEffect(Collider2D col)
    {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy == null) return;
        PDamage(enemy);
    }
    public override void TimedEffect()
    {
        RandomPush();
    }
}
