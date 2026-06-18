using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music")]
    public AudioClip ambientMusic;

    [Header("SFX")]
    public AudioClip treasureCollectClip;
    public AudioClip treasureMissClip;
    public AudioClip obstacleHitClip;
    public AudioClip swimLoopClip;
    public AudioClip gameOverClip;
    public AudioClip sheildActivatedClip;
    public AudioClip sheildDeactivatedClip;


    private const string VolumePrefKey = "SFXVolume";
    private const string MusicPrefKey = "MusicVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadVolumeSettings();
    }

    private void Start()
    {
        PlayAmbientMusic();
    }

    private void LoadVolumeSettings()
    {
        float sfxVolume = PlayerPrefs.GetFloat(VolumePrefKey, 0.8f);
        float musicVolume = PlayerPrefs.GetFloat(MusicPrefKey, 0.5f);

        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float value)
    {
        if (sfxSource != null)
            sfxSource.volume = value;
        PlayerPrefs.SetFloat(VolumePrefKey, value);
    }

    public void SetMusicVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;
        PlayerPrefs.SetFloat(MusicPrefKey, value);
    }

    public void PlayAmbientMusic()
    {
        if (musicSource != null && ambientMusic != null)
        {
            musicSource.clip = ambientMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayTreasureCollect()
    {
        PlaySFX(treasureCollectClip);
    }


    public void PlayTreasureMiss()
    {
        PlaySFX(treasureMissClip);
    }

    public void PlayObstacleHit()
    {
        PlaySFX(obstacleHitClip);
    }

    public void PlaySwimLoop()
    {
        PlaySFX(swimLoopClip);
    }

    public void PlayGameOver()
    {
        PlaySFX(gameOverClip);
    }

    public void PlaySheildActivate()
    {
        PlaySFX(sheildActivatedClip);
    }

    public void PlaySheildDeactivate()
    {
        PlaySFX(sheildDeactivatedClip);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
