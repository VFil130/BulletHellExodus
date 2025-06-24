using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    public static EnemyManager Instance { get; private set; }
    private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private List<EnemyToSpawn> enemiesToSpawn = new List<EnemyToSpawn>();
    private WaveController waveController;
    [SerializeField] private float spawnRadius = 10f;

    [System.Serializable]
    public class EnemyToSpawn
    {
        public Enemy enemyPrefab;
        public float spawnDelay;
        public float nextSpawn;
        public int spawnLevel;
        public int spawnCount;
        public MonoBehaviour spawnStrategy;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        waveController = FindFirstObjectByType<WaveController>();
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("Игрок не найден");
            }

        }

        FindAllEnemies();
    }

    private void FindAllEnemies()
    {
        enemies.Clear();
        Enemy[] foundEnemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in foundEnemies)
        {
            enemies.Add(enemy);
        }
    }

    void Update()
    {
        RemoveDeadEnemies();
        SpawnEnemies();
    }

    private void RemoveDeadEnemies()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = enemies[i];
            if (enemy != null && enemy.IsDead)
            {
                Destroy(enemy.gameObject); 
                enemies.RemoveAt(i);
            }
        }
    }
    private void SpawnEnemies()
    {
        if (waveController == null)
        {
            Debug.LogError("Контроллер волны не найден");
            return;
        }

        for (int i = enemiesToSpawn.Count - 1; i >= 0; i--)
        {
            EnemyToSpawn spawnData = enemiesToSpawn[i];

            if (spawnData.spawnLevel <= waveController.waveLevel)
            {
                if (Time.time >= spawnData.nextSpawn)
                {
                    if (spawnData.enemyPrefab != null && spawnData.spawnStrategy != null)
                    {
                        if (spawnData.spawnStrategy is ISpawn spawnStrategy)
                        {
                            Vector2 randomDirection = Random.insideUnitCircle.normalized;
                            Vector2 spawnPosition = (Vector2)playerTransform.position + randomDirection * spawnRadius;
                            Enemy[] spawnedEnemies = spawnStrategy.Spawn(spawnData.enemyPrefab, spawnPosition, spawnData.spawnCount);

                            if (spawnedEnemies != null)
                            {
                                foreach (Enemy newEnemy in spawnedEnemies)
                                {
                                    if (newEnemy != null)
                                    {
                                        enemies.Add(newEnemy);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError("SpawnStrategy не реализует ISpawn");
                        }
                    }
                else
                {
                    Debug.LogWarning("Надо назначить префаб врага или тратегию спавна");
                }

                    spawnData.nextSpawn = Time.time + spawnData.spawnDelay;
                }
            }
        }
    }
    public Transform PlayerTransform
    {
        get
        {
            return playerTransform;
        }
    }

    public List<Enemy> Enemies
    {
        get { return enemies; }
    }
    public float SpawnRadius
    {
        get { return spawnRadius; }
    }
}