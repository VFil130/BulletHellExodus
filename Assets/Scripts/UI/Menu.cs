using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]private TMP_Text[] resCount;
    [SerializeField] private GameObject mapsPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject charactersPanel;
    [SerializeField] private GameObject abilitiesPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Image backGround;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    public string sceneToLoad = "Game";
    public void Start()
    {
        UpdateResourceCount();
        HideAllPanels();
        SetupSoundSliders();
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
    private void HideAllPanels()
    {
        if (mapsPanel != null) mapsPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (charactersPanel != null) charactersPanel.SetActive(false);
        if (abilitiesPanel != null) abilitiesPanel.SetActive(false);
        if (mainPanel !=null) mainPanel.SetActive(false);
    }
}
