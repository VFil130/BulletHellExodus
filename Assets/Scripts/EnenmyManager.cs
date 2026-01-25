using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    public static EnemyManager Instance { get; private set; }
    private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private List<EnemyToSpawn> enemiesToSpawn = new List<EnemyToSpawn>();
    private WaveController waveController;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private GameObject damageNumberPrefab;
    [SerializeField] private Color physicalColor = Color.red;
    [SerializeField] private Color magicalColor = Color.blue;
    [SerializeField] private Color criticalColor = Color.magenta;
    [SerializeField] private Color clearColor = Color.yellow;
    [SerializeField] private float maxEnemyDistance = 15f;
    [SerializeField] private float teleportDistance = 20f;
    [SerializeField] private float pullSpeed = 5f;
    [SerializeField] private float checkDistanceInterval = 1f;
    public event Action<Vector3, float, DamageType> OnDamageTaken;

    [System.Serializable]
    public class EnemyToSpawn
    {
        public Enemy enemyPrefab;
        public float spawnDelay;
        public float nextSpawn;
        public int spawnLevel;
        public int spawnCount;
        public SpawnStrategy spawnStrategy;
    }
    public enum DamageType
    {
        Physical,
        Magical,
        Critical,
        Clear
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
        OnDamageTaken += ShowDamageNumber;
    }
    void OnDestroy()
    {
        if (Instance == this)
        {
            OnDamageTaken -= ShowDamageNumber;
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
        InvokeRepeating(nameof(CheckEnemiesDistance), checkDistanceInterval, checkDistanceInterval);
    }
    private void CheckEnemiesDistance()
    {
        if (playerTransform == null) return;

        foreach (Enemy enemy in enemies)
        {
            if (enemy == null || enemy.IsDead) continue;

            float distance = Vector3.Distance(enemy.transform.position, playerTransform.position);

            if (distance > teleportDistance)
            {
                Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
                Vector2 newPosition = (Vector2)playerTransform.position + randomDirection * spawnRadius;
                enemy.transform.position = newPosition;
            }
            else if (distance > maxEnemyDistance)
            {
                enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, playerTransform.position, pullSpeed * Time.deltaTime);
            }
        }
    }
    public void TriggerDamage(Vector3 position, float damage, DamageType type)
    {
        OnDamageTaken?.Invoke(position, damage, type);
    }
    #region DamageNumber
    private void ShowDamageNumber(Vector3 position, float damage, DamageType type)
    {
        if (damageNumberPrefab == null)
        {
            Debug.LogWarning("DamageNumberPrefab не назначен!");
            return;
        }

        GameObject damageNumberObj = Instantiate(damageNumberPrefab, position, Quaternion.identity);

        TextMeshPro textMesh = damageNumberObj.GetComponentInChildren<TextMeshPro>();
        if (textMesh == null)
        {
            Debug.LogError("DamageNumber prefab не содержит TextMeshPro!");
            Destroy(damageNumberObj);
            return;
        }

        textMesh.text = Mathf.RoundToInt(damage).ToString();
        textMesh.color = GetColorByType(type);
        textMesh.alpha = 1f;

        Vector3 endPosition = position + new Vector3(
            UnityEngine.Random.Range(-0.3f, 0.3f),
            1f,
            0f
        );

        Sequence sequence = DOTween.Sequence();
        sequence.Append(damageNumberObj.transform.DOMove(endPosition, 1f).SetEase(Ease.OutCubic));
        sequence.Join(textMesh.DOFade(0f, 1f).SetEase(Ease.InQuad));
        sequence.OnComplete(() => Destroy(damageNumberObj));
    }

    private Color GetColorByType(DamageType type)
    {
        return type switch
        {
            DamageType.Physical => physicalColor,
            DamageType.Magical => magicalColor,
            DamageType.Critical => criticalColor,
            DamageType.Clear => clearColor,
            _ => Color.white
        };
    }
    #endregion
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
                        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
                        Vector2 spawnPosition = (Vector2)playerTransform.position + randomDirection * spawnRadius;

                        Enemy[] spawnedEnemies = spawnData.spawnStrategy.Spawn(
                            spawnData.enemyPrefab,
                            spawnPosition,
                            spawnData.spawnCount
                        );

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
                        Debug.LogWarning("Надо назначить префаб врага или стратегию спавна");
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
    public void SetPlayerTransform(Transform transform)
    {
        playerTransform = transform;
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