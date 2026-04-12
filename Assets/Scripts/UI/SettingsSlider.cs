using UnityEngine;
using UnityEngine.UI;

public class SettingsSlider : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;

    private void Start()
    {
        if (musicSlider != null)
        {
            musicSlider.value = SoundManager.Instance.GetMusicVolume();
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = SoundManager.Instance.GetSFXVolume();
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
        if (uiSlider != null)
        {
            uiSlider.value = SoundManager.Instance.GetUIVolume();
            uiSlider.onValueChanged.AddListener(OnUIVolumeChanged);
        }
    }

    private void OnMusicVolumeChanged(float value)
    {
        SoundManager.Instance.SetMusicVolume(value);
    }
    private void OnSFXVolumeChanged(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }
    private void OnUIVolumeChanged(float value)
    {
        SoundManager.Instance.SetUIVolume(value);
    }
    private void OnDestroy()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
        if (uiSlider != null)
            uiSlider.onValueChanged.RemoveListener(OnUIVolumeChanged);
    }
}