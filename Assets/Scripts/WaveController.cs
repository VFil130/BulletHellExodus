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
    public List<ResourceZone> resourceZones;
    [Range(1, 5)] public int zonesActive = 1;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text notyfytxt;
    private float elapsedTime = 0f;
    public static WaveController instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (resourceZones == null || resourceZones.Count == 0)
        {
            resourceZones = FindObjectsOfType<ResourceZone>().ToList();
        }

        ActivateRandomZones();
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
            notyfytxt.text += $"Evacuation Active!\n";
        }
        ActivateRandomZones();
        ShowNotify();
    }
    private void ActivateRandomZones()
    {
        foreach (var zone in resourceZones)
        {
            zone.DeactivateZone();
        }

        if (resourceZones != null && resourceZones.Count > 0)
        {
            List<ResourceZone> validZones = resourceZones.Where(z => z != null).ToList();
            if (validZones.Count > 0)
            {
                List<ResourceZone> zonesToActivate = validZones.OrderBy(x => Random.value).Take(zonesActive).ToList();
                notyfytxt.text += "Active Zones: ";
                foreach (var zone in zonesToActivate)
                {
                    zone.ActivateZone();
                    int zoneIndex = resourceZones.IndexOf(zone);
                    notyfytxt.text += $"Zone {zoneIndex}: {zone.resource} ";
                }
            }
            else
            {
                Debug.LogWarning("Нет активных ResourceZone в списке!");
            }
        }
    }
    private void HideNotify()
    {
        notyfytxt.text = "";
        notyfytxt.gameObject.SetActive(false);
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
    private void ShowNotify()
    {
        notyfytxt.gameObject.SetActive(true);
        Invoke(nameof(HideNotify), 10f);
    }
}