using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Image mapImage;
    [SerializeField] private Menu menu;
    public void Start()
    {
        if (sceneName == "Game")
        {
            OnClick();
        }
    }
    public void OnClick()
    {
        menu.ShowRecords(sceneName, mapImage);
    }
}