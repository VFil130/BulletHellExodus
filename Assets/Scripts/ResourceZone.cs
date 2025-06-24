using UnityEngine;

public class ResourceZone : MonoBehaviour
{
    [SerializeField] private Resources resource;
    [SerializeField] private float amount;
    private float captureTimer;
    [SerializeField] private float captureTime;
    [SerializeField] private bool isActive;

    public void Capturing()
    {
        if (isActive)
        {
            captureTimer += Time.deltaTime;

            if (captureTimer >= captureTime)
            {
                isActive = false;
                captureTime = 0;
                TakenResources takenResources = FindFirstObjectByType<TakenResources>();
                takenResources.TakeResources(resource, amount);
            }
        }
    }

    public void DeactivateZone()
    {
        isActive = false;
        captureTimer = 0;
    }

    public void ActivateZone()
    {
        isActive = true;
        captureTimer = 0;
    }
}