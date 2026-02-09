using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.ShaderGraph.Internal;
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
    public void DropNo()
    {
        needDrop = false;
    }
    private void OnDestroy()
    {
        if (!needDrop)
        {
            return;
        }
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
