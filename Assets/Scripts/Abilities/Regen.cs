using UnityEngine;

public class Regen : AbilityStats, IAbility
{
    private float healAmount = 1;
    public float AbilityInterval { get; set; } = 1f;
    public Character Character;
    void Start()
    {
        Character = GetComponentInParent<Character>();
    }

    public bool CanUseAbility()
    {
        return true;
    }
    public void UseAbility()
    {
        Character.Heal(healAmount);
    }
}
