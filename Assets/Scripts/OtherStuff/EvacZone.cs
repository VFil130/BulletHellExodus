using Mono.Cecil;
using UnityEngine;

public class EvacZone : MonoBehaviour
{
    [SerializeField] private bool isActive = false;
    [SerializeField] private float captureTimer;
    [SerializeField] private float captureTime = 3;
    [SerializeField] private Transform filler;

    private bool playerInZone = false;
    public static EvacZone instance { get; private set; }

    public void Start()
    {
        instance = this;
        UpdateFiller();
    }
    private void Update()
    {
        if (!isActive) return;
        if (playerInZone)
        {
            captureTimer += Time.deltaTime;
            if (captureTimer >= captureTime)
            {
                captureTimer = captureTime;
            }
        }
        else
        {
            captureTimer -= Time.deltaTime;
            if (captureTimer <= 0)
            {
                captureTimer = 0;
            }
        }

        UpdateFiller();
        if (captureTimer >= captureTime)
        {
            SceneController.instance.Evacuate();
        }
    }
    private void UpdateFiller()
    {
        float progress = Mathf.Clamp01(captureTimer / captureTime);
        Vector3 newScale = filler.localScale;
        newScale.x = progress;
        newScale.y = progress;
        filler.localScale = newScale;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }
    public void Activate()
    {
        isActive = true;
        Debug.Log("EVAC ACTIVE");
    }
}
