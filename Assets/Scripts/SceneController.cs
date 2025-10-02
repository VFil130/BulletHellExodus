using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour // Любая логика связанная с переходом между сценами
{
    public static SceneController instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SceneChange(string name)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(name);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Evacuate()
    {
        MainInventory.instance.AddResources(TakenResources.instance.Inventory);
        MainInventory.instance.SaveResources();
        SceneChange("Menu");
    }
}
