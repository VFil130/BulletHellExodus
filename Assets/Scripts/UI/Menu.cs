using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private TMP_Text[] resCount;
    [SerializeField] private TMP_Text charInfo;
    [SerializeField] private Image charImage;
    [SerializeField] private GameObject mapsPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject charactersPanel;
    [SerializeField] private GameObject abilitiesPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Image backGround;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TMP_Text recordsText;
    [SerializeField] private Image mapImage;
    public string sceneToLoad = "Game";
    public void Start()
    {
        UpdateResourceCount();
        HideAllPanels();
        SetupSoundSliders();
        LoadSelectedCharacter();
    }
    public void Update()
    {
        UpdateResourceCount();
    }
    public void StartGame()
    {
        SceneController.instance.SceneChange(sceneToLoad);
    }
    public void SetSceneToLoad(string name)
    {
        sceneToLoad = name;
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void UpdateResourceCount()
    {
        for (int i = 0; i < resCount.Length; i++)
        {
            ValResources resource = (ValResources)i;
            float amount = MainInventory.instance.GetResourceAmount(resource);
            resCount[i].text = amount.ToString();
        }
    }
    public void SetBackGround(Sprite back)
    {
        backGround.sprite = back;
    }
    private void SetupSoundSliders()
    {
        if (SoundManager.Instance != null)
        {
            musicSlider.value = SoundManager.Instance.GetMusicVolume();
            sfxSlider.value = SoundManager.Instance.GetSFXVolume();

            musicSlider.onValueChanged.AddListener(SoundManager.Instance.SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);
        }
    }
    public void ShowPanel(string panelName)
    {
        HideAllPanels();

        switch (panelName)
        {
            case "maps":
                if (mapsPanel != null) mapsPanel.SetActive(true);
                break;
            case "settings":
                if (settingsPanel != null) settingsPanel.SetActive(true);
                break;
            case "characters":
                if (charactersPanel != null) charactersPanel.SetActive(true);
                break;
            case "abilities":
                if (abilitiesPanel != null) abilitiesPanel.SetActive(true);
                break;
        }
    }
    public void TakeCharacter(Character character)
    {
        MainInventory.instance.SetCharacter(character);
        LoadCharInfo(character);

        PlayerPrefs.SetString("SelectedCharacter", character.gameObject.name);
        PlayerPrefs.Save();
    }
    private void LoadSelectedCharacter()
    {
        string savedCharacterName = PlayerPrefs.GetString("SelectedCharacter", "");
        if (string.IsNullOrEmpty(savedCharacterName)) return;

        GameObject characterPrefab = Resources.Load<GameObject>($"Characters/{savedCharacterName}");
        if (characterPrefab == null) return;

        Character character = characterPrefab.GetComponent<Character>();
        if (character != null)
        {
            MainInventory.instance.SetCharacter(character);
            LoadCharInfo(character);
        }
    }
    public void LoadCharInfo(Character character)
    {
        charInfo.text = character.GetCharacterInfo();
        charImage.sprite = character.sprite.sprite;
    }
    private void HideAllPanels()
    {
        if (mapsPanel != null) mapsPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (charactersPanel != null) charactersPanel.SetActive(false);
        if (abilitiesPanel != null) abilitiesPanel.SetActive(false);
        if (mainPanel !=null) mainPanel.SetActive(false);
    }
    public void ShowRecords(string sceneName, Image image)
    {
        int maxWave = PlayerPrefs.GetInt($"MaxWaveRecord_{sceneName}", 0);
        float maxDamage = PlayerPrefs.GetFloat($"MaxDamageRecord_{sceneName}", 0);
        recordsText.text = $"Максимальная волна {maxWave}\n\nМаксимальный урон {maxDamage}";
        mapImage.sprite = image.sprite;
    }
}
