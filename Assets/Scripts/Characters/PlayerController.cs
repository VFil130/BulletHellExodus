using UnityEngine;

public class PlayerController : Character
{
    private Vector2 _mousePosition;
    private bool isMoving = false;
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        _mousePosition = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    void FixedUpdate()
    {
        CharacterMovement();
    }

    private void CharacterMovement()
    {
        if (isMoving)
        {
            Vector2 cameraCenter = (Vector2)mainCamera.transform.position;
            Vector2 direction = (_mousePosition - cameraCenter).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            sprite.transform.rotation = Quaternion.identity;

            transform.Translate(new Vector2(direction.x, direction.y) * speed * Time.fixedDeltaTime, Space.World);
            if (direction.x < 0)
            {
                sprite.flipX = true;
            }
            else if (direction.x > 0)
            {
                sprite.flipX = false;
            }
        }
    }

    public float ReturnSpeed()
    {
        return speed;
    }
}