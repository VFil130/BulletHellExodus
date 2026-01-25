using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    private PlayerController playerController;

    void Start()
    {
        player = FindFirstObjectByType<Character>().transform;

    }
    void FixedUpdate()
    {
        transform.position = player.position;
    }
}