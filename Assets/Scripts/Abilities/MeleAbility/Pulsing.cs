using System.Collections;
using UnityEngine;
using DG.Tweening;
public class Pulsing : Melee
{
    [SerializeField]private float scaleTime;
    protected override void Punch(Enemy enemy)
    {
        enemy.TakeMageDamage(currentDamage);
    }
    private void Start()
    {
        StartCoroutine(PulseScale(scaleTime));
    }
}
