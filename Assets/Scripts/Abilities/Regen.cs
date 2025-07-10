using UnityEngine;

public class Regen : AbilityStats
{
    private float healAmount = 1;
    public Character Character;
    void Start()
    {
        Character = GetComponentInParent<Character>();
    }
    public override bool CanUseAbility()
    {
        return true;
    }
    public override void UseAbility()
    {
        Character.Heal(healAmount);
    }
}
