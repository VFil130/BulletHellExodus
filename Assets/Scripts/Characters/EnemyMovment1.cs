using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovment : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    private bool isMoving = true;
    void Update()
    {
        float distance = Vector2.Distance(EnemyManager.Instance.PlayerTransform.transform.position, transform.position);
        if (distance <= 0.2)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    void FixedUpdate()
    {
        CharacterMovment();
    }

    private void CharacterMovment()
    {
        Vector2 direction = (EnemyManager.Instance.PlayerTransform.position - transform.position).normalized;
        if (isMoving)
        {
            transform.Translate(new Vector2(direction.x, direction.y) * enemyData.MoveSpeed * Time.fixedDeltaTime);
        }
    }
}