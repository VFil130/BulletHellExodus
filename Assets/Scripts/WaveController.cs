using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class WaveController : MonoBehaviour
{
    [SerializeField] private float baseHealthIncrease = 0.2f;
    [SerializeField] private float baseArmourIncrease = 0.1f;
    public int waveLevel;
    private float waveTimer;
    public float timeBetweenWaves;
    [SerializeField] private int maxActiveZones = 3;
    [SerializeField] private float spawnRadiusMin = 10f;
    [SerializeField] private float spawnRadiusMax = 20f;
    [SerializeField] private List<GameObject> resourceZonePrefabs;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text notifyText;
    private float elapsedTime = 0f;
    public static WaveController instance;

    private List<ResourceZone> activeResourceZones = new List<ResourceZone>();
    private Queue<ResourceZone> zoneQueue = new Queue<ResourceZone>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnResourceZone();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        UpdateTimerDisplay();
        waveTimer += Time.deltaTime;
        if (waveTimer >= timeBetweenWaves)
        {
            WaveLevelUp();
        }
    }

    private void WaveLevelUp()
    {
        waveLevel += 1;
        waveTimer = 0;
        waveText.text = waveLevel.ToString();

        if (waveLevel == 2)
        {
            EvacZone.instance.Activate();
            ShowNotify("EVACUATION ACTIVE!");
        }

        SpawnResourceZone();
        EnemyManager.Instance.UpdateSpawnDelaysForWave(waveLevel);
    }

    private void SpawnResourceZone()
    {
        if (resourceZonePrefabs == null || resourceZonePrefabs.Count == 0 || centerPoint == null)
        {
            Debug.LogWarning("Не назначены префабы зон или центр карты!");
            return;
        }

        GameObject selectedPrefab = resourceZonePrefabs[Random.Range(0, resourceZonePrefabs.Count)];

        Vector2 spawnPos = GetRandomPositionFromCenter();
        GameObject newZoneObj = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        ResourceZone newZone = newZoneObj.GetComponent<ResourceZone>();

        if (newZone != null)
        {
            activeResourceZones.Add(newZone);
            zoneQueue.Enqueue(newZone);
            ShowNotify($"Resource zone appeared! (+{newZone.GetAmount()} {newZone.GetResourceType()})");

            if (activeResourceZones.Count > maxActiveZones)
            {
                RemoveOldestZone();
            }
        }
    }

    private Vector2 GetRandomPositionFromCenter()
    {
        Vector2 center = centerPoint.position;
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distance = Random.Range(spawnRadiusMin, spawnRadiusMax);

        float x = center.x + Mathf.Cos(angle) * distance;
        float y = center.y + Mathf.Sin(angle) * distance;

        return new Vector2(x, y);
    }

    private void RemoveOldestZone()
    {
        if (zoneQueue.Count > 0)
        {
            ResourceZone oldestZone = zoneQueue.Dequeue();
            if (oldestZone != null && activeResourceZones.Contains(oldestZone))
            {
                activeResourceZones.Remove(oldestZone);
                ShowNotify("Resource zone disappeared!");
                Destroy(oldestZone.gameObject);
            }
        }
    }

    public void OnZoneCollected(ResourceZone zone)
    {
        if (activeResourceZones.Contains(zone))
        {
            activeResourceZones.Remove(zone);

            if (zoneQueue.Contains(zone))
            {
                zoneQueue = new Queue<ResourceZone>(zoneQueue.Where(z => z != zone));
            }

            ShowNotify($"Collected {zone.GetAmount()} {zone.GetResourceType()}!");
            Destroy(zone.gameObject);
        }
    }

    private void ShowNotify(string message)
    {
        if (notifyText != null)
        {
            notifyText.text = message;
            notifyText.gameObject.SetActive(true);
            CancelInvoke(nameof(HideNotify));
            Invoke(nameof(HideNotify), 5f);
        }
    }

    private void HideNotify()
    {
        if (notifyText != null)
        {
            notifyText.text = "";
            notifyText.gameObject.SetActive(false);
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public (float healthMult, float armourMult) GetWaveMultipliers()
    {
        int tenWaveCount = waveLevel / 10;
        float healthMult = 1f + (baseHealthIncrease * tenWaveCount);
        float armourMult = 1f + (baseArmourIncrease * tenWaveCount);
        return (healthMult, armourMult);
    }
}