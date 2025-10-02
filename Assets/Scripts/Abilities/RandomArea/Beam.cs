using UnityEngine;

public class Beam : AreaEffect
{
    public override void DoEffect(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            MDamage(col.GetComponent<Enemy>());
            PDamage(col.GetComponent<Enemy>());
        }
    }
}
