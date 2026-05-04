using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState
    {
        Gameplay,
        Pause,
        GameOver,
        LevelUp
    }

    public GameState currentState;
    public GameState previousState;

    [Header("UI")]
    public GameObject pauseScreen;
    public GameObject levelUpScreen;
    public GameObject endGameScreen;
    public TMP_Text endGameInfo;
    private float totalDamge = 0;
    private float totalKills = 0;
    private bool gameIsWin = false;
    public bool choosingUpgrade;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Лишний" + this + "Удалён");
            Destroy(gameObject);
        }
        DisableScreens();
        CreateCharacter();
    }
    void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                break;
            case GameState.Pause:
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                break;
            case GameState.LevelUp:
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f;
                    levelUpScreen.SetActive(true);
                    AbilityManager.instance.ShowAbilities();
                }
                break;

            default:
                Debug.LogWarning("Не подходящее состояние игры");
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }
    public void EndGame()
    {
        if(currentState != GameState.GameOver)
        {
            AddInfoToEndText(TakenResources.instance.ReturnCapturedResources());
            AddDamageInfo();
            AddKillsInfo();
            previousState = currentState;
            SaveRecords();
            ChangeState(GameState.GameOver);
            Time.timeScale = 0f;
            endGameScreen.SetActive(true);
        }
    }
    public void AddTotalDamage(float dmg)
    {
        totalDamge += dmg;
    }
    public void AddTotalKills()
    {
        totalKills += 1;
    }
    public void AddDamageInfo()
    {
        string text = "\nНанесённый урон: ";
        text += totalDamge.ToString();
        AddInfoToEndText(text);
    }
    public void AddKillsInfo()
    {
        string text = "\nВсего убийств: ";
        text += totalKills.ToString();
        AddInfoToEndText(text);
    }
    public void SetWinLose(bool state)
    {
        gameIsWin = state;
    }
    public void ToMenu()
    {
        if (gameIsWin)
        {
            SceneController.instance.Evacuate();
        }
        else
        {
            SceneController.instance.SceneChange("Menu");
        }
    }
    public void AddInfoToEndText(string txt)
    {
        endGameInfo.text += txt;
    }
    public void PauseGame()
    {
        if (currentState != GameState.Pause)
        {
            previousState = currentState;
            ChangeState(GameState.Pause);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            Debug.Log("Игра остановленна");
        }
    }
    public void ResumeGame()
    {
        if (currentState == GameState.Pause)
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            Debug.Log("Игра продолжается");
        }
    }

    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentState == GameState.Pause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    void DisableScreens()
    {
        endGameScreen.SetActive(false);
        pauseScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
    }
    public void EndLevelUp()
    {
        if (currentState != GameState.Gameplay)
        {
            choosingUpgrade = false;
            AbilityManager.instance.HideAbilities();
            Time.timeScale = 1f;
            levelUpScreen.SetActive(false);
            ChangeState(GameState.Gameplay);
        }
    }
    public void CreateCharacter()
    {
        if (MainInventory.instance.character != null)
        {
            Instantiate(MainInventory.instance.character);

            AbilityManager.instance ??= FindObjectOfType<AbilityManager>();
            AbilityManager.instance?.Init();
        }
    }
    #region Records System
    private void SaveRecords()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        int currentWave = WaveController.instance.waveLevel;

        int savedMaxWave = PlayerPrefs.GetInt($"MaxWaveRecord_{sceneName}", 0);
        float savedMaxDamage = PlayerPrefs.GetFloat($"MaxDamageRecord_{sceneName}", 0);
        float savedMaxKills = PlayerPrefs.GetFloat($"MaxKillsRecord_{sceneName}", 0);

        if (currentWave > savedMaxWave)
        {
            PlayerPrefs.SetInt($"MaxWaveRecord_{sceneName}", currentWave);
        }

        if (totalDamge > savedMaxDamage)
        {
            PlayerPrefs.SetFloat($"MaxDamageRecord_{sceneName}", totalDamge);
        }

        if (totalKills > savedMaxKills)
        {
            PlayerPrefs.SetFloat($"MaxKillsRecord_{sceneName}", totalKills);
        }

        PlayerPrefs.Save();
    }

    public int GetMaxWaveRecord()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        return PlayerPrefs.GetInt($"MaxWaveRecord_{sceneName}", 0);
    }

    public float GetMaxDamageRecord()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        return PlayerPrefs.GetFloat($"MaxDamageRecord_{sceneName}", 0);
    }

    public float GetMaxKillsRecord()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        return PlayerPrefs.GetFloat($"MaxKillsRecord_{sceneName}", 0);
    }
    #endregion
}
