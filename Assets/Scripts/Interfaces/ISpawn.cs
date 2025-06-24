using UnityEngine;

public interface ISpawn
{
    Enemy[] Spawn(Enemy enemy, Vector2 spawnPosition, int spawnCount);
}
