using System.Collections;
using UnityEngine;

public class TrollWarLordBoss : Enemy
{
    [Header("Troll Boss Settings")]
    [SerializeField] private Transform clubTransform;
    [SerializeField] private float clubRotateSpeed = 180f;
    [SerializeField] private float clubPushForce = 5f;
    [SerializeField] private float clubAttackCooldown = 0.5f;
    [SerializeField] private float pushDuration = 0.2f;

    private float lastDamageTime = 0;
    [SerializeField] private CustomCollider clubCollider;

    public override void Update()
    {
        base.Update();
        if (IsDead) return;

        RotateClub();
        CheckClubDamage();
    }

    private void RotateClub()
    {
        if (clubTransform != null)
        {
            clubTransform.Rotate(0, 0, clubRotateSpeed * Time.deltaTime);
        }
    }

    private void CheckClubDamage()
    {
        if (Time.time < lastDamageTime + clubAttackCooldown) return;
        if (clubCollider == null) return;

        Collider2D[] hits = clubCollider.CheckCollisions(LayerMask.GetMask("Player"), 20);

        foreach (Collider2D hit in hits)
        {
            if (hit != null && hit.CompareTag("Player"))
            {
                Character character = hit.GetComponent<Character>();
                if (character != null)
                {
                    character.TakeDamage(currentDamage, 0);
                    lastDamageTime = Time.time;
                }

                StartCoroutine(PushPlayer(hit.transform));
                break;
            }
        }
    }

    private IEnumerator PushPlayer(Transform playerTransform)
    {
        Vector2 pushDirection = (playerTransform.position - transform.position).normalized;
        float elapsedTime = 0;
        Vector2 startPosition = playerTransform.position;

        while (elapsedTime < pushDuration)
        {
            float t = elapsedTime / pushDuration;
            Vector2 newPosition = startPosition + pushDirection * clubPushForce * t;
            playerTransform.position = newPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public override void DieEffect()
    {
        base.DieEffect();
    }
}