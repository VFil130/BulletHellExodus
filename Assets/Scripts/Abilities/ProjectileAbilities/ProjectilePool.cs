using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;

    [Header("Настройки пула")]
    public int initialPoolSize = 20;
    public int maxPoolSize = 200;

    private GameObject projectilePrefab;
    private Queue<GameObject> availableProjectiles = new Queue<GameObject>();
    private List<GameObject> allProjectiles = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Раскомментируйте если нужно между сценами
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetProjectilePrefab(GameObject prefab)
    {
        if (projectilePrefab == null && prefab != null)
        {
            projectilePrefab = prefab;
            InitializePool();
        }
    }

    void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewProjectile();
        }
        Debug.Log($"Пул снарядов инициализирован: {initialPoolSize} объектов");
    }

    void CreateNewProjectile()
    {
        if (allProjectiles.Count >= maxPoolSize)
        {
            Debug.LogWarning("Достигнут максимальный размер пула!");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.SetParent(transform);
        projectile.SetActive(false);

        // Настраиваем скрипт снаряда для работы с пулом
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetPool(this);
        }

        availableProjectiles.Enqueue(projectile);
        allProjectiles.Add(projectile);
    }

    public GameObject GetProjectile()
    {
        if (availableProjectiles.Count == 0)
        {
            CreateNewProjectile();
        }

        if (availableProjectiles.Count > 0)
        {
            GameObject projectile = availableProjectiles.Dequeue();
            projectile.SetActive(true);
            return projectile;
        }

        Debug.LogWarning("Не удалось получить снаряд из пула");
        return null;
    }

    public void ReturnProjectile(GameObject projectile)
    {
        if (projectile != null)
        {
            projectile.SetActive(false);
            availableProjectiles.Enqueue(projectile);
        }
    }

    // Методы для мониторинга
    public int GetActiveProjectilesCount()
    {
        int count = 0;
        foreach (var projectile in allProjectiles)
        {
            if (projectile.activeInHierarchy) count++;
        }
        return count;
    }

    public int GetTotalProjectilesCount()
    {
        return allProjectiles.Count;
    }

    public void ReturnAllProjectiles()
    {
        foreach (var projectile in allProjectiles)
        {
            if (projectile.activeInHierarchy)
            {
                ReturnProjectile(projectile);
            }
        }
    }
}