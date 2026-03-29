using System.Collections.Generic;
using UnityEngine;

public class MeleCast : AbilityStats
{
    [SerializeField] protected GameObject MelePrefab;
    protected List<GameObject> currentMeleePrefabs = new List<GameObject>();
    void Update()
    {
        for (int i = currentMeleePrefabs.Count - 1; i >= 0; i--)
        {
            if (currentMeleePrefabs[i] == null)
            {
                currentMeleePrefabs.RemoveAt(i);
                continue;
            }

            Melee melee = currentMeleePrefabs[i].GetComponent<Melee>();
            if (melee != null && melee.destroy)
            {
                Destroy(currentMeleePrefabs[i]);
                currentMeleePrefabs.RemoveAt(i);
            }
        }
    }
    public void CreateMele()
    {
        GameObject meleeObj = Instantiate(MelePrefab, abilityCaster.transform.position, abilityCaster.transform.rotation);
        meleeObj.transform.SetParent(abilityCaster.transform);
        Melee meleeScript = meleeObj.GetComponent<Melee>();
        meleeScript.Initialize(this);
        currentMeleePrefabs.Add(meleeObj);
    }

    public void CreateMeleInLocalPosition(float x, float y)
    {
        Vector3 offset = new Vector3(x, y, 0);
        GameObject meleeObj = Instantiate(MelePrefab, abilityCaster.transform.position, abilityCaster.transform.rotation);
        meleeObj.transform.SetParent(abilityCaster.transform);
        meleeObj.transform.localPosition = offset;
        meleeObj.transform.localRotation = Quaternion.identity;
        Melee meleeScript = meleeObj.GetComponent<Melee>();
        meleeScript.Initialize(this);
        currentMeleePrefabs.Add(meleeObj);
    }
}