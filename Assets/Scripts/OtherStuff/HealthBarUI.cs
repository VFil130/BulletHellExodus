using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField]private Image healthBar;
    [SerializeField]private Character character;
    public void Start()
    {
        character = FindFirstObjectByType<Character>();
    }
    void Update()
    {
        UpdateHealthBar();
    }
    void UpdateHealthBar()
    {
        healthBar.fillAmount = character.ReturnHealth();
    }
}
