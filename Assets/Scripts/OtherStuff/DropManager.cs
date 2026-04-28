using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float baseDropRate;
        public bool useFixedDropRate = false;
    }

    public List<Drops> drops;
    public bool needDrop = true;
    public float spreadRadius = 0.5f;
    [SerializeField] private float dropRateIncreasePerWave = 0.1f;
    [SerializeField] private int maxWaveForDrop = 30;
    public void DropNo()
    {
        needDrop = false;
    }
    private void OnDestroy()
    {
        if (!needDrop) return;
        if (!gameObject.scene.isLoaded) return;

        int currentWave = WaveController.instance != null ? WaveController.instance.waveLevel : 0;
        int cappedWave = Mathf.Min(currentWave, maxWaveForDrop);
        float waveMultiplier = 1f + (dropRateIncreasePerWave * cappedWave);

        foreach (Drops drop in drops)
        {
            float finalDropRate;

            if (drop.useFixedDropRate)
            {
                finalDropRate = drop.baseDropRate;
            }
            else
            {
                finalDropRate = drop.baseDropRate * waveMultiplier;
            }

            float remainingChance = finalDropRate;

            while (remainingChance > 0)
            {
                float roll = Random.Range(0f, 100f);

                if (roll <= remainingChance)
                {
                    Vector3 randomOffset = Random.insideUnitCircle * spreadRadius;
                    Vector3 dropPosition = transform.position + new Vector3(randomOffset.x, 0, randomOffset.y);
                    Instantiate(drop.itemPrefab, dropPosition, Quaternion.identity);
                    remainingChance -= 100f;
                }
                else
                {
                    break;
                }
            }
        }
    }
}