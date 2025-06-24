using System.Collections;
using UnityEngine;

public class CircelBlast : ProjectileCast, IAbility
{
    [SerializeField] public float AbilityInterval { get; set; } = 4f;
    [SerializeField] private int radius = 360;
    [SerializeField] private int numberOfProjectiles = 8;
    void Awake()
    {
        AbilityInterval = SetCastInterval();
    }
    public bool CanUseAbility()
    {
        return true;
    }

    public void UseAbility()
    {
        StartCoroutine(ShootMultipleTimes());
    }
    protected override void SetProjectilesTransform()
    {
        float characterDirection = abilityCaster.transform.eulerAngles.z;
        float startAngle = -radius / 2f;
        float endAngle = radius / 2f;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float currentAngle;
            if (numberOfProjectiles == 1)
            {
                currentAngle = 0f;
            }
            else
            {
                float t = (float)i / (numberOfProjectiles - 1);
                currentAngle = Mathf.Lerp(startAngle, endAngle, t);
            }

            float finalAngle = characterDirection + currentAngle;

            Vector2 direction = new Vector2(
                Mathf.Cos(finalAngle * Mathf.Deg2Rad),
                Mathf.Sin(finalAngle * Mathf.Deg2Rad)
            ).normalized;

            float angleInDegrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angleInDegrees);
            Vector3 position = abilityCaster.transform.position;

            CreateProjectile(position, rotation);
        }
    }
}