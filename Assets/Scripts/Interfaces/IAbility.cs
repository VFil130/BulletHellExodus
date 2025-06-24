using UnityEngine;

public interface IAbility
{
    float AbilityInterval { get; set; }
    void UseAbility();
    bool CanUseAbility();
}
