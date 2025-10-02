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

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            return;
        }
        float randomNumber = Random.Range(0f, 100f);
        foreach (Drops drop in drops)
        {
            if (randomNumber <= drop.dropRate)
            {
                Instantiate(drop.itemPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
