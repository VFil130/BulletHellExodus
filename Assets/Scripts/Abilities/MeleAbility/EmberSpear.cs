using UnityEngine;

public class EmberSpear : Melee
{
    void Start()
    {
        Poke();
    }
    protected override void Punch(Enemy enemy)
    {
        enemy.TakeMageDamage(currentDamage);
        PushEnemy(enemy);
        enemy.EmberEffect(currentDamage/2);
    }
}
