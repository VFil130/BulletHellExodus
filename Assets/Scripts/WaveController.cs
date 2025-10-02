using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class WaveController : MonoBehaviour
{
    public int waveLevel;
    private float waveTimer;
    public float timeBetweenWaves;
    public List<ResourceZone> resourceZones;
    [Range(1, 5)] public int zonesActive = 1;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text timer; 
    private float elapsedTime = 0f;

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
        ActivateRandomZones();
        if (waveLevel == 2)
        {
            EvacZone.instance.Activate();
        }
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

                foreach (var zone in zonesToActivate)
                {
                    zone.ActivateZone();
                }
            }
            else
            {
                Debug.LogWarning("Нет активных ResourceZone в списке!");
            }
        }
    }
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60); 
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}