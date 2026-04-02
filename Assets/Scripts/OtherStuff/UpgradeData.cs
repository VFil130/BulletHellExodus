using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public UpgradeStat stat;
    public float valuePerLevel;
    public List<UpgradeCost> costs;
}

public enum UpgradeStat
{
    Damage,
    Radius,
    Duration,
    Pierce,
    Shoots,
    Power,
    TickRate,
    Interval,
    Speed
}

[Serializable]
public class UpgradeCost
{
    public List<ResourceCost> resources;
}

[Serializable]
public class ResourceCost
{
    public ValResources resourceType;
    public int amount;
}