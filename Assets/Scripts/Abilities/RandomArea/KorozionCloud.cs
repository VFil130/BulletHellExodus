using UnityEngine;

public class KorozionCloud : AreaEffect
{
    public override void DoEffect(Collider2D col)
    {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy == null) return;

        PDamage(enemy);
        CorosionApply(enemy);
    }
}
