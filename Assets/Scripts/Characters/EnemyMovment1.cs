using UnityEngine;

public class EnemyMovment : MonoBehaviour
{
    public enum MovementType
    {
        Melee,
        Ranged,
        Stationary,
        LineFormation
    }

    public Enemy enemyData;
    public MovementType movementType = MovementType.Melee;

    [Header("Movement Settings")]
    [SerializeField] private float meleeStopDistance = 0.2f;
    [SerializeField] private float rangedStopDistance = 3f;
    [SerializeField] private float attackRange = 5f;

    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;
    private Vector2 lineMovementDirection;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = EnemyManager.Instance.PlayerTransform;
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 toPlayer = playerTransform.position - transform.position;

        switch (movementType)
        {
            case MovementType.Melee:
                HandleMeleeMovement(toPlayer);
                break;

            case MovementType.Ranged:
                HandleRangedMovement(toPlayer);
                break;

            case MovementType.Stationary:
                break;

            case MovementType.LineFormation:
                MoveInLineFormation();
                break;
        }

        UpdateSpriteDirection(toPlayer.normalized);
    }

    private void HandleMeleeMovement(Vector2 toPlayer)
    {
        if (toPlayer.magnitude > meleeStopDistance)
        {
            MoveTowardsPlayer(toPlayer.normalized);
        }
    }

    private void HandleRangedMovement(Vector2 toPlayer)
    {
        float distance = toPlayer.magnitude;
        bool playerInRange = distance <= attackRange;
        bool tooClose = distance < rangedStopDistance;
        bool tooFar = distance > attackRange;

        if (playerInRange && tooClose)
        {
            Vector2 directionAway = -toPlayer.normalized;
            transform.position = Vector2.MoveTowards(
                transform.position,
                (Vector2)transform.position + directionAway,
                enemyData.currentMoveSpeed * Time.deltaTime
            );
        }
        else if (tooFar)
        {
            MoveTowardsPlayer(toPlayer.normalized);
        }
    }

    private void MoveTowardsPlayer(Vector2 direction)
    {
        transform.Translate(direction * enemyData.currentMoveSpeed * Time.fixedDeltaTime);
    }

    private void MoveInLineFormation()
    {
        transform.Translate(lineMovementDirection * enemyData.currentMoveSpeed * Time.fixedDeltaTime);
    }

    private void UpdateSpriteDirection(Vector2 direction)
    {
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void SetLineFormationDirection(Vector2 direction)
    {
        movementType = MovementType.LineFormation;
        lineMovementDirection = direction.normalized;
    }
}