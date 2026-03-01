using UnityEngine;

[CreateAssetMenu(fileName = "LineSpawnStrategy", menuName = "Enemy/Spawn Strategy/Line")]
public class LineSpawnStrategy : SpawnStrategy
{
    [SerializeField] private float spacing = 2f;

    public override Enemy[] Spawn(Enemy enemyPrefab, Vector2 spawnPosition, int spawnCount)
    {
        if (enemyPrefab == null || spawnCount <= 0)
        {
            return null;
        }

        Enemy[] spawnedEnemies = new Enemy[spawnCount];

        Transform playerTransform = EnemyManager.Instance.PlayerTransform;
        if (playerTransform == null)
        {
            return null;
        }

        Vector2 toPlayer = ((Vector2)playerTransform.position - spawnPosition).normalized;
        Vector2 perpendicular = new Vector2(-toPlayer.y, toPlayer.x).normalized;

        float halfLength = (spawnCount - 1) * spacing * 0.5f;

        for (int i = 0; i < spawnCount; i++)
        {
            float offset = -halfLength + i * spacing;
            Vector2 enemyPosition = spawnPosition + perpendicular * offset;
            Enemy newEnemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
            spawnedEnemies[i] = newEnemy;

            var enemyMovement = newEnemy.GetComponent<EnemyMovment>();
            if (enemyMovement != null)
            {
                enemyMovement.SetLineFormationDirection(toPlayer);
            }
        }

        return spawnedEnemies;
    }
}