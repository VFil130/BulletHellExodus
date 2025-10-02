using UnityEngine;

public class ExpOrb : MonoBehaviour, ICollectable
{
    public int experiecneGranted;
    public void Collect()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        player.IncreaseExperience(experiecneGranted);
        Destroy(gameObject);
    }
}
