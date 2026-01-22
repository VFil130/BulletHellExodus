using UnityEngine;

[CreateAssetMenu(fileName = "CircleSpawnStrategy", menuName = "Enemy/Spawn Strategy/Circle")]
public class CircleSpawnStrategy : SpawnStrategy
{
    [SerializeField] private float circleRadius = 5f;

    public override Enemy[] Spawn(Enemy enemyPrefab, Vector2 spawnPosition, int spawnCount)
    {
        if (enemyPrefab == null)
        {
            return null;
        }

        Enemy[] spawnedEnemies = new Enemy[spawnCount];

        for (int i = 0; i < spawnCount; i++)
        {
            float angle = i * (360f / spawnCount);
            float radians = angle * Mathf.Deg2Rad;

            Vector2 offset = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * circleRadius;
            Vector2 enemyPosition = spawnPosition + offset;

            Enemy newEnemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
            spawnedEnemies[i] = newEnemy;
        }

        return spawnedEnemies;
    }
}