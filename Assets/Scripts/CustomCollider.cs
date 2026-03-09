using UnityEngine;

public enum HitShapeType
{
    Circle,
    Box
}

public class CustomCollider : MonoBehaviour
{
    [SerializeField] private HitShapeType shapeType = HitShapeType.Circle;

    [Header("Position Offset")]
    [SerializeField] private Vector2 offset = Vector2.zero;

    [Header("Circle Settings")]
    [SerializeField] private float circleRadius = 0.2f;

    [Header("Box Settings")]
    [SerializeField] private Vector2 boxSize = new Vector2(0.5f, 0.2f);
    [SerializeField] private bool rotateBoxWithProjectile = true;

    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private Color gizmoColor = new Color(1f, 0f, 0f, 0.3f);

    private Collider2D[] hitResults = new Collider2D[10];

    private Vector2 GetPositionWithOffset()
    {
        return (Vector2)transform.position + offset;
    }

    public Collider2D[] CheckCollisions(LayerMask targetMask)
    {
        System.Array.Clear(hitResults, 0, hitResults.Length);

        switch (shapeType)
        {
            case HitShapeType.Circle:
                Physics2D.OverlapCircle(
                    GetPositionWithOffset(),
                    circleRadius,
                    new ContactFilter2D().NoFilter(),
                    hitResults
                );
                break;

            case HitShapeType.Box:
                float angle = rotateBoxWithProjectile ? transform.eulerAngles.z : 0f;
                Physics2D.OverlapBox(
                    GetPositionWithOffset(),
                    boxSize,
                    angle,
                    new ContactFilter2D().NoFilter(),
                    hitResults
                );
                break;
        }

        return hitResults;
    }

    public Collider2D[] CheckCollisions(LayerMask targetMask, int maxResults)
    {
        if (hitResults.Length != maxResults)
        {
            hitResults = new Collider2D[maxResults];
        }

        System.Array.Clear(hitResults, 0, hitResults.Length);

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(targetMask);
        filter.useLayerMask = true;

        switch (shapeType)
        {
            case HitShapeType.Circle:
                Physics2D.OverlapCircle(
                    GetPositionWithOffset(),
                    circleRadius,
                    filter,
                    hitResults
                );
                break;

            case HitShapeType.Box:
                float angle = rotateBoxWithProjectile ? transform.eulerAngles.z : 0f;
                Physics2D.OverlapBox(
                    GetPositionWithOffset(),
                    boxSize,
                    angle,
                    filter,
                    hitResults
                );
                break;
        }

        return hitResults;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = gizmoColor;
        Vector2 gizmoPosition = GetPositionWithOffset();

        switch (shapeType)
        {
            case HitShapeType.Circle:
                Gizmos.DrawWireSphere(gizmoPosition, circleRadius);
                break;

            case HitShapeType.Box:
                Gizmos.matrix = Matrix4x4.TRS(gizmoPosition, transform.rotation, Vector3.one);
                Gizmos.DrawWireCube(Vector3.zero, boxSize);
                Gizmos.matrix = Matrix4x4.identity;
                break;
        }
    }
}