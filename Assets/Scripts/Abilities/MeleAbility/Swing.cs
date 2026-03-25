using System.Collections;
using UnityEngine;
using DG.Tweening;
public class RelicSword : Melee
{
    protected override void Punch(Enemy enemy)
    {
        enemy.TakeClearDamage(currentDamage);
        PushEnemy(enemy);
    }
    private void Start()
    {
        StartCoroutine(Swinging());
    }
}
