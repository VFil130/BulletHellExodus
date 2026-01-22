using UnityEngine;

[CreateAssetMenu(fileName = "GroupSpawnStrategy", menuName = "Enemy/Spawn Strategy/Group")]
public class GroupSpawnStrategy : SpawnStrategy
{
    [SerializeField] private float groupRadius = 3f;

    public override Enemy[] Spawn(Enemy enemyPrefab, Vector2 spawnPosition, int spawnCount)
    {
        if (enemyPrefab == null)
        {
            return null;
        }

        Enemy[] spawnedEnemies = new Enemy[spawnCount];

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * groupRadius;
            Vector2 enemyPosition = spawnPosition + randomOffset;

            Enemy newEnemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
            spawnedEnemies[i] = newEnemy;
        }

        return spawnedEnemies;
    }
}