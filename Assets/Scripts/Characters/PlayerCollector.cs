using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    [SerializeField] private float magnetSpeed = 8f;
    [SerializeField] private float magnetRange = 3f;

    private void Update()
    {
        Collider2D[] orbs = Physics2D.OverlapCircleAll(transform.position, magnetRange);

        foreach (Collider2D orb in orbs)
        {
            MagnetCollectable magnetOrb = orb.GetComponent<MagnetCollectable>();
            if (magnetOrb != null && !magnetOrb.IsBeingCollected)
            {
                magnetOrb.StartMovingToPlayer(transform, magnetSpeed);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
    }
}