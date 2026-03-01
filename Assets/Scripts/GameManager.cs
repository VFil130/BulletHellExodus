using TMPro;
using UnityEngine;

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
        CreateCharacter();
        DisableScreens();    
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
        string text = "\nCurrentDamage: ";
        text += totalDamge.ToString();
        AddInfoToEndText(text);
    }
    public void AddKillsInfo()
    {
        string text = "\nCurrentKills: ";
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
            AbilityManager.instance.Init();
        }
    }
}
