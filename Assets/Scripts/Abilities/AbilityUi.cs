using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUi : MonoBehaviour
{
    [SerializeField] private Image[] icons = new Image[10];
    public void ChangeImages (int index, Sprite icon)
    { 
        icons[index].sprite = icon;
    }
}
