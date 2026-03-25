using UnityEngine;

public class ResourceZone : MonoBehaviour
{
    [SerializeField] private ValResources resourceType;
    [SerializeField] private float amount;
    [SerializeField] private float collectionTime = 3f;
    [SerializeField] private Transform filler;
    [SerializeField] private float detectionRadius = 1f;

    private float collectionTimer = 0f;
    private bool isBeingCollected = false;
    private GameObject player;
    private bool isActive = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (filler != null)
        {
            filler.localScale = Vector3.zero;
        }
    }

    void Update()
    {
        if (!isActive || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            Capturing();
        }
        else
        {
            if (isBeingCollected)
            {
                isBeingCollected = false;
                collectionTimer = 0f;
                if (filler != null)
                {
                    filler.localScale = Vector3.zero;
                }
            }
        }
    }

    public void Capturing()
    {
        if (!isActive) return;

        if (!isBeingCollected)
        {
            isBeingCollected = true;
            collectionTimer = 0f;
        }

        collectionTimer += Time.deltaTime;
        UpdateFiller();

        if (collectionTimer >= collectionTime)
        {
            CollectZone();
        }
    }

    private void CollectZone()
    {
        isActive = false;

        TakenResources takenResources = FindFirstObjectByType<TakenResources>();
        if (takenResources != null)
        {
            takenResources.TakeResources(resourceType, amount);
        }

        if (WaveController.instance != null)
        {
            WaveController.instance.OnZoneCollected(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void UpdateFiller()
    {
        if (filler != null)
        {
            float progress = Mathf.Clamp01(collectionTimer / collectionTime);
            Vector3 newScale = filler.localScale;
            newScale.x = progress;
            newScale.y = progress;
            filler.localScale = newScale;
        }
    }

    public void DeactivateZone()
    {
        isActive = false;
        isBeingCollected = false;
        collectionTimer = 0;
        if (filler != null)
        {
            filler.localScale = Vector3.zero;
        }
    }

    public void ActivateZone()
    {
        isActive = true;
        isBeingCollected = false;
        collectionTimer = 0;
    }

    public ValResources GetResourceType()
    {
        return resourceType;
    }

    public float GetAmount()
    {
        return amount;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}