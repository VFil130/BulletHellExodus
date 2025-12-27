using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TextMeshPro damageText;
    [SerializeField] private float floatDistance = 1f;
    [SerializeField] private float animationDuration = 1f;

    public void Initialize(float damage, Color color, Vector3 position)
    {
        transform.position = position;
        damageText.text = Mathf.RoundToInt(damage).ToString();
        damageText.color = color;
        damageText.alpha = 1f;

        Vector3 endPosition = position + new Vector3(
            Random.Range(-0.3f, 0.3f),
            floatDistance,
            0f
        );

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(endPosition, animationDuration).SetEase(Ease.OutCubic));
        sequence.Join(damageText.DOFade(0f, animationDuration).SetEase(Ease.InQuad));
        sequence.OnComplete(() => Destroy(gameObject));
    }
}