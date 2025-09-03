using UnityEngine;
using UnityEngine.UI;

public class CharacterScriptableObject : ScriptableObject
{
    [SerializeField]
    AbilityStats startAbility;
    public AbilityStats StartAbility { get => startAbility; private set => startAbility = value;}

    [SerializeField]
    float maxHealth;
    public float MaxHealt { get => maxHealth; private set => maxHealth = value; }

    [SerializeField]
    Image icon;
    public Image Icon { get => icon; private set => icon = value; }

    [SerializeField]
    float speed;
    public float Speed { get => speed; private set => speed = value; }

    [SerializeField]
    float physArmour;
    public float PhysArmour { get => physArmour; private set => physArmour = value; }

    [SerializeField]
    float mageArmour;
    public float MageArmour { get => mageArmour; private set => mageArmour = value; }
}
