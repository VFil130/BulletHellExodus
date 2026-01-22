using UnityEngine;

public class DamageArea : AreaEffect
{
    public override void DoEffect(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            PDamage(col.GetComponent<Enemy>());
            col.GetComponent<Enemy>().CorosionEffect(currentDamage);
        }
    }
}
