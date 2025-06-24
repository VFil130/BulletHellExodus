using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothing;
    private PlayerController playerController;

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }
    void FixedUpdate()
    {
        smoothing = playerController.ReturnSpeed();
        transform.position = Vector2.Lerp(transform.position, player.position, smoothing * Time.deltaTime);
    }
}