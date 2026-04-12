using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHower : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlayButtonHover();
    }

    public void ButtonClickMusik()
    {
        SoundManager.Instance.PlayButtonClick();
    }
}