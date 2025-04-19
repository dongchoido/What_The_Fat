using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource bgmSource;

    [Header("Audio Clips")]
    public AudioClip jumpClip;
    public AudioClip shootClip;
    public AudioClip spawnClip;
    public AudioClip deathClip;
    public AudioClip coinClip;
    public AudioMixer audioMixer; // chứa exposed param: "SFXVolume"

    public bool isSFXEnabled = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SyncSettingsFromPrefs();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null && isSFXEnabled && sfxSource.volume > 0f)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayBGM(AudioClip music)
    {
        if (bgmSource != null && music != null)
        {
            bgmSource.clip = music;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }

    public void SyncSettingsFromPrefs()
    {
        bool bgmOn = PlayerPrefs.GetInt("BGMEnabled", 1) == 1;
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);

        bool sfxOn = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (bgmSource != null)
        {
            bgmSource.volume = bgmOn ? bgmVolume : 0f;
        }

        isSFXEnabled = sfxOn;

        if (audioMixer != null)
        {
            float db = Mathf.Log10(Mathf.Clamp(sfxOn ? sfxVolume : 0.0001f, 0.0001f, 1f)) * 20f;
            audioMixer.SetFloat("SFXVolume", db);
        }
    }

    // Convenience methods
    public void PlayJumpSound() => PlaySFX(jumpClip);
    public void PlayShootSound() => PlaySFX(shootClip);
    public void PlaySpawnSound() => PlaySFX(spawnClip);
    public void PlayDeathSound() => PlaySFX(deathClip);
    public void PlayCoinSound() => PlaySFX(coinClip);

}
