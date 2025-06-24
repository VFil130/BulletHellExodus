using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WaveController : MonoBehaviour
{
    public int waveLevel;
    private float waveTimer;
    public float timeBetweenWaves;
    public List<ResourceZone> resourceZones;
    [Range(1, 5)] public int zonesActive = 1;

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
        ActivateRandomZones();
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
}