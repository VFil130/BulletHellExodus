using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string translationKey;
    private TextMeshProUGUI textComponent;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        UpdateText(0);
    }

    public void UpdateText(int id)
    {
        if (LanguageManager.Instance != null && textComponent != null)
        {
            int lang = PlayerPrefs.GetInt("Language", id);
            textComponent.text = LanguageManager.Instance.GetTranslation(translationKey, lang);
        }
    }
}