using UnityEngine;

public class ExpOrb : MagnetCollectable
{
    [SerializeField] private int experiecneGranted = 10;

    public override void Collect()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.IncreaseExperience(experiecneGranted);
        }
        Destroy(gameObject);
    }
}
