using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSourcePrefab;

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;

    [SerializeField] private AudioClip enemyDamageSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudio()
    {
        if (musicSource == null)
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            PlayMenuMusic();
        }
        else if (scene.name == "Game")
        {
            PlayGameplayMusic();
        }
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayGameplayMusic()
    {
        PlayMusic(gameplayMusic);
    }
    private void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat("MusicVolume", musicSource.volume);
            PlayerPrefs.Save();
        }
    }
    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", Mathf.Clamp01(volume));
        PlayerPrefs.Save();
    }
    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
    public void PlayEnemyDamageSound(Vector3 position)
    {
        PlaySFXAtPosition(enemyDamageSound, position);
    }

    public void PlaySFXAtPosition(AudioClip clip, Vector3 position)
    {
        if (clip == null || sfxSourcePrefab == null)
        {
            Debug.LogWarning("SoundManager: Clip or prefab is missing!");
            return;
        }

        AudioSource tempSource = Instantiate(sfxSourcePrefab, position, Quaternion.identity);
        tempSource.clip = clip;
        tempSource.volume = GetSFXVolume();
        tempSource.Play();

        StartCoroutine(DestroyAfterPlay(tempSource.gameObject, clip.length));
    }

    public void PlaySFX(AudioClip clip)
    {
        PlaySFXAtPosition(clip, Vector3.zero);
    }

    private IEnumerator DestroyAfterPlay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}