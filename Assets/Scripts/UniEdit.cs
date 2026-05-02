#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class ToolsMenu
{
    [MenuItem("Tools/Очистить PlayerPrefs")]
    static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs очищены через меню!");
    }
}
#endif