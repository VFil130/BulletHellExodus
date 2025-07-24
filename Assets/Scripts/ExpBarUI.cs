using UnityEngine;
using UnityEngine.UI;

public class ExpBarUI : MonoBehaviour
{
    [SerializeField] private Image expBar;
    [SerializeField] private Character character;
    void Update()
    {
        UpdateExpBar();
    }
    void UpdateExpBar()
    {
        expBar.fillAmount = character.ExpToNextLevel();
    }
}

