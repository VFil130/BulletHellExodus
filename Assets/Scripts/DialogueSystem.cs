using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject dialogPanel;
    public Image characterSprite;
    public TMP_Text dialogText;
    public float textSpeed = 0.05f;

    [Header("Character Sprites")]
    public Sprite warriorSprite;
    public Sprite merchantSprite;
    public Sprite mageSprite;

    private bool isTyping = false;
    private bool dialogActive = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        HideDialog();
        TestMageDialog(1);
    }

    void Update()
    {
        if (dialogActive && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                SkipTyping();
            }
            else
            {
                HideDialog();
            }
        }
    }

    public void StartDialog(string characterName, int dialogId)
    {
        string text = GetDialogText(characterName, dialogId);
        if (!string.IsNullOrEmpty(text))
        {
            ShowDialog();
            ShowCharacterDialog(characterName, text);
        }
        else
        {
            Debug.LogError($"Dialog not found for {characterName} with id {dialogId}!");
        }
    }

    string GetDialogText(string characterName, int dialogId)
    {
        TextAsset xmlFile = Resources.Load<TextAsset>("Dialogs");
        if (xmlFile == null)
        {
            Debug.LogError("Dialogs.xml not found in Resources!");
            return "";
        }

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlFile.text);

        XmlNode characterNode = xmlDoc.SelectSingleNode($"//character[@name='{characterName}']");
        if (characterNode == null)
        {
            Debug.LogError($"Character {characterName} not found!");
            return "";
        }

        XmlNode dialogNode = characterNode.SelectSingleNode($"node[@id='{dialogId}']");
        return dialogNode?.InnerText.Trim();
    }

    void ShowCharacterDialog(string characterName, string text)
    {
        // Устанавливаем спрайт персонажа
        SetCharacterSprite(characterName);

        // Показываем текст
        typingCoroutine = StartCoroutine(TypeText(text));
    }

    void SetCharacterSprite(string characterName)
    {
        switch (characterName)
        {
            case "Warrior":
                characterSprite.sprite = warriorSprite;
                break;
            case "Merchant":
                characterSprite.sprite = merchantSprite;
                break;
            case "Mage":
                characterSprite.sprite = mageSprite;
                break;
            default:
                Debug.LogWarning($"No sprite found for character: {characterName}");
                break;
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        isTyping = false;
    }

    void ShowDialog()
    {
        dialogPanel.SetActive(true);
        dialogActive = true;
    }

    void HideDialog()
    {
        dialogPanel.SetActive(false);
        dialogActive = false;
        isTyping = false;
    }

    // Методы для быстрого тестирования
    public void TestWarriorDialog(int id) => StartDialog("Warrior", id);
    public void TestMerchantDialog(int id) => StartDialog("Merchant", id);
    public void TestMageDialog(int id) => StartDialog("Mage", id);
}