using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnStrategy", menuName = "Enemy/Spawn Strategy")]
public abstract class SpawnStrategy : ScriptableObject, ISpawn
{
    public abstract Enemy[] Spawn(Enemy enemyPrefab, Vector2 spawnPosition, int spawnCount);
}