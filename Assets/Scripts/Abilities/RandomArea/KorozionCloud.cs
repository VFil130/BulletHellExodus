using UnityEngine;

public class KorozionCloud : AreaEffect
{
    public override void DoEffect(Collider2D col)
    {
        PDamage(col.GetComponent<Enemy>());
        CorosionApply(col.GetComponent<Enemy>());
    }
}
