using UnityEngine;

public class ResourceZone : MonoBehaviour
{
    [SerializeField] private Resources resource;
    [SerializeField] private float amount;
    [SerializeField] private float captureTimer;
    [SerializeField] private float captureTime;
    [SerializeField] private bool isActive;
    [SerializeField] private Transform filler;

    public void Capturing()
    {
        if (isActive)
        {
            captureTimer += Time.deltaTime;
            UpdateFiller();
            if (captureTimer >= captureTime)
            {
                DeactivateZone();
                filler.localScale = Vector3.zero;
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
    private void UpdateFiller()
    {
        float progress = Mathf.Clamp01(captureTimer / captureTime);
        Vector3 newScale = filler.localScale;
        newScale.x = progress;
        newScale.y = progress;
        filler.localScale = newScale;
    }
}