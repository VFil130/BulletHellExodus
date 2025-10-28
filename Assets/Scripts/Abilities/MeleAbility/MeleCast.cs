using System.Collections.Generic;
using UnityEngine;

public class MeleCast : AbilityStats
{
    [SerializeField] protected GameObject MelePrefab;
    protected AbilityCaster abilityCaster;
    private GameObject currentMelePrefab;

    void Start()
    {
        abilityCaster = GetComponentInParent<AbilityCaster>();
    }
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
}