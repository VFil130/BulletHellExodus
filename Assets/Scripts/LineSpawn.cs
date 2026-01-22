using UnityEngine;

[CreateAssetMenu(fileName = "LineSpawnStrategy", menuName = "Enemy/Spawn Strategy/Line")]
public class LineSpawnStrategy : SpawnStrategy
{
    [SerializeField] private float spacing = 2f;
    [SerializeField] private LineDirection direction = LineDirection.Horizontal;

    public enum LineDirection
    {
        Horizontal,
        Vertical
    }

    public override Enemy[] Spawn(Enemy enemyPrefab, Vector2 spawnPosition, int spawnCount)
    {
        if (enemyPrefab == null)
        {
            return null;
        }

        Enemy[] spawnedEnemies = new Enemy[spawnCount];

        float startOffset = -(spawnCount - 1) * spacing * 0.5f;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 enemyPosition = spawnPosition;

            if (direction == LineDirection.Horizontal)
            {
                enemyPosition += new Vector2(startOffset + i * spacing, 0);
            }
            else
            {
                enemyPosition += new Vector2(0, startOffset + i * spacing);
            }

            Enemy newEnemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
            spawnedEnemies[i] = newEnemy;
        }

        return spawnedEnemies;
    }
}
