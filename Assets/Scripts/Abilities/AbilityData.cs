using UnityEngine;

[CreateAssetMenu(fileName = "AbilitySciptableObject", menuName = "ScriptableObjects/Ability")]
public class AbilityData : ScriptableObject
{
    [SerializeField]
    private float castInterval;
    public float CastInterval { get => castInterval; private set => castInterval = value; }

    [SerializeField]
    private float speed;
    public float Speed { get => speed; private set => speed = value; }

    [SerializeField]
    private float damage;
    public float Damage { get => damage; private set => damage = value; }

    [SerializeField]
    private GameObject abilityPrefab;
    public GameObject AbilityPrefab { get => abilityPrefab; private set => abilityPrefab = value; }

    [SerializeField]
    private float pierce;
    public float Pierce { get => pierce; private set => pierce = value; }

    [SerializeField]
    int level;
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    AbilityData nextLevelData;
    public AbilityData NextLevelData { get => nextLevelData;private set => NextLevelData = value; }

    [SerializeField]
    string description;
    public string Description { get => description; private set => description = value; }
}
