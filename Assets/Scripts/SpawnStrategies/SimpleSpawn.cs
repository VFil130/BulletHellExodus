using UnityEngine;

public class SimpleSpawn : MonoBehaviour, ISpawn
{
    public Enemy[] Spawn(Enemy enemyPrefab, Vector2 spawnPosition, int spawnCount)
    {
        Enemy[] spawnedEnemies = new Enemy[spawnCount];

        if (enemyPrefab != null)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Enemy newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                spawnedEnemies[i] = newEnemy; 
            }
        }
        else
        {
            Debug.Log("Нет ссылки на префаб для стратегии спавна");
        }
        return spawnedEnemies;
    }
}