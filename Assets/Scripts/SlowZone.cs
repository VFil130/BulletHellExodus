using UnityEngine;
using static Character;

public class SlowZone : MonoBehaviour
{
    [SerializeField] private float slowPercent = 50f;
    [SerializeField] private string slowEffectName = "zone_slow";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Character character))
        {
            character.AddEffect(slowEffectName, 0.1f, slowPercent, true, 1);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out Character character))
        {
            character.AddEffect(slowEffectName, 0.1f, slowPercent, true, 1);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Character character))
        {
            ActiveEffect effect = character.GetEffect(slowEffectName);
            if (effect != null)
            {
                effect.duration = 0;
            }
        }
    }
}