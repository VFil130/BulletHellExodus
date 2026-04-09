using UnityEngine;

public abstract class MagnetCollectable : MonoBehaviour, ICollectable
{
    [SerializeField] protected AnimationCurve magnetCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] protected float lifeTime = 30f;

    protected Transform target;
    protected bool isMovingToPlayer = false;
    protected float magnetProgress = 0f;
    protected float currentLifeTime;
    public bool IsBeingCollected => isMovingToPlayer;

    protected virtual void Start()
    {
        currentLifeTime = lifeTime;
    }

    protected virtual void Update()
    {
        if (isMovingToPlayer && target != null)
        {
            MoveToPlayer();
        }
        else if (!isMovingToPlayer)
        {
            UpdateLifeTime();
        }
    }

    protected virtual void UpdateLifeTime()
    {
        currentLifeTime -= Time.deltaTime;
        if (currentLifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void StartMovingToPlayer(Transform playerTransform, float speed)
    {
        if (isMovingToPlayer) return;

        target = playerTransform;
        isMovingToPlayer = true;
        magnetProgress = 0f;
    }

    protected virtual void MoveToPlayer()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= 0.2f)
        {
            Collect();
            return;
        }

        magnetProgress += Time.deltaTime;
        float t = magnetCurve.Evaluate(Mathf.Clamp01(magnetProgress));

        Vector2 direction = (target.position - transform.position).normalized;
        float speed = Mathf.Lerp(3f, 15f, t);

        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public abstract void Collect();
}