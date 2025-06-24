using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }
    [SerializeField]
    float maxHealth;
    public float MaxHealth { get => maxHealth; private set => maxHealth = value; 
    }
    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value; }
    [SerializeField]
    float mageArmour;
    public float MageArmour { get => mageArmour; private set => mageArmour = value; }
    [SerializeField]
    float physArmour;
    public float PhysArmour { get => physArmour; private set => physArmour = value; }
}
