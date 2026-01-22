using System.Collections.Generic;
using UnityEngine;

public class MeleCast : AbilityStats
{
    [SerializeField] protected GameObject MelePrefab;
    protected GameObject currentMelePrefab;
    void Update()
    {
        if (currentMelePrefab != null)
        {
            Melee melee = currentMelePrefab.GetComponent<Melee>();
            if (melee.destroy)
            {
                Destroy(currentMelePrefab);
            }
        }
    }
    public void CreateMele()
    {
        currentMelePrefab = Instantiate(MelePrefab, abilityCaster.transform.position, abilityCaster.transform.rotation);
        currentMelePrefab.transform.SetParent(abilityCaster.transform);
        Melee meleeScript = currentMelePrefab.GetComponent<Melee>();
        meleeScript.Initialize(this);
    }
    public void CreateMeleInLocalPosition(float x, float y)
    {
        Vector3 offset = new Vector3(x, y, 0);
        currentMelePrefab = Instantiate(MelePrefab, abilityCaster.transform);
        currentMelePrefab.transform.localPosition = offset;
        currentMelePrefab.transform.localRotation = Quaternion.identity;
        Melee meleeScript = currentMelePrefab.GetComponent<Melee>();
        meleeScript.Initialize(this);
    }
}