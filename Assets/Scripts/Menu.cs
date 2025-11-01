using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]private TMP_Text[] resCount;
    public void Start()
    {
        UpdateResourceCount();
    }
    public void StartGame(string name)
    {
        SceneController.instance.SceneChange(name);
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
}
