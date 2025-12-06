using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    [System.Serializable]
    public class Translation
    {
        public string key;
        public string russian;
        public string english;
    }

    public List<Translation> translations = new List<Translation>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLanguage();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLanguage(int languageIndex)
    {
        // 0 - Russian, 1 - English
        PlayerPrefs.SetInt("Language", languageIndex);
        ApplyLanguage();
    }

    void LoadLanguage()
    {
        if (!PlayerPrefs.HasKey("Language"))
        {
            PlayerPrefs.SetInt("Language", 0); // По умолчанию русский
        }
        ApplyLanguage();
    }

    void ApplyLanguage()
    {
        LocalizedText[] allTexts = FindObjectsOfType<LocalizedText>();
        foreach (LocalizedText text in allTexts)
        {
            text.UpdateText(PlayerPrefs.GetInt("Language"));
        }
    }

    public string GetTranslation(string key, int languageIndex)
    {
        Translation translation = translations.Find(t => t.key == key);
        if (translation != null)
        {
            return languageIndex == 0 ? translation.russian : translation.english;
        }
        return $"[{key}]";
    }
}