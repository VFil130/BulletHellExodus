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
        public float dropRate;
    }

    public List<Drops> drops;
    public bool needDrop = true;
    public float spreadRadius = 0.5f;

    public void DropNo()
    {
        needDrop = false;
    }

    private void OnDestroy()
    {
        if (!needDrop) return;
        if (!gameObject.scene.isLoaded) return;

        float waveMultiplier = GetWaveMultiplier();
        float randomNumber = Random.Range(0f, 100f);

        foreach (Drops drop in drops)
        {
            float finalDropRate = drop.dropRate * waveMultiplier;
            if (randomNumber <= finalDropRate)
            {
                Vector3 randomOffset = Random.insideUnitCircle * spreadRadius;
                Vector3 dropPosition = transform.position + new Vector3(randomOffset.x, 0, randomOffset.y);
                Instantiate(drop.itemPrefab, dropPosition, Quaternion.identity);
            }
        }
    }

    private float GetWaveMultiplier()
    {
        if (WaveController.instance == null) return 1f;

        int waveLevel = WaveController.instance.waveLevel;
        float multiplier = 1f + (waveLevel / 15f);
        return Mathf.Min(multiplier, 3f);
    }
}