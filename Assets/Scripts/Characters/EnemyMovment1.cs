using UnityEngine;

public class EnemyMovment : MonoBehaviour
{
    public enum MovementType
    {
        Melee,
        Ranged,
        Stationary
    }

    public Enemy enemyData;
    public MovementType movementType = MovementType.Melee;

    [Header("Movement Settings")]
    [SerializeField] private float meleeStopDistance = 0.2f;
    [SerializeField] private float rangedStopDistance = 3f;
    [SerializeField] private float attackRange = 5f;

    private bool isMoving = true;
    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = EnemyManager.Instance.PlayerTransform;
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector2.Distance(playerTransform.position, transform.position);

        switch (movementType)
        {
            case MovementType.Melee:
                isMoving = distance > meleeStopDistance;
                break;

            case MovementType.Ranged:
                bool playerInRange = distance <= attackRange;
                bool tooClose = distance < rangedStopDistance;
                bool tooFar = distance > attackRange;

                if (playerInRange && tooClose)
                {
                    Vector2 directionAway = (transform.position - playerTransform.position).normalized;
                    transform.position = Vector2.MoveTowards(
                        transform.position,
                        (Vector2)transform.position + directionAway,
                        enemyData.currentMoveSpeed * Time.deltaTime
                    );
                }
                else if (tooFar)
                {
                    isMoving = true;
                }
                else
                {
                    isMoving = false;
                }
                break;

            case MovementType.Stationary:
                isMoving = false;
                break;
        }
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;

        CharacterMovement();
    }

    private void CharacterMovement()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        if (movementType == MovementType.Melee && isMoving)
        {
            transform.Translate(new Vector2(direction.x, direction.y) * enemyData.currentMoveSpeed * Time.fixedDeltaTime);
        }
        else if (movementType == MovementType.Ranged && isMoving && Vector2.Distance(playerTransform.position, transform.position) > attackRange)
        {
            transform.Translate(new Vector2(direction.x, direction.y) * enemyData.currentMoveSpeed * Time.fixedDeltaTime);
        }
    }
}