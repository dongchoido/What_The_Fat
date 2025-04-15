using UnityEngine;

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
    public bool isSFXEnabled = true; // gán bởi Pause.cs

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại qua các scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play one-shot sound effect
    public void PlaySFX(AudioClip clip)
{
    if (clip != null && sfxSource != null && isSFXEnabled && sfxSource.volume > 0f)
    {
        sfxSource.PlayOneShot(clip);
    }
}
    

    // Play background music (loop)
    public void PlayBGM(AudioClip music)
    {
        if (bgmSource != null && music != null)
        {
            bgmSource.clip = music;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    // Stop BGM
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
    float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);

    bool sfxOn = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
    float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

    if (bgmSource != null)
    {
        bgmSource.volume = bgmOn ? bgmVolume : 0f;
    }

    isSFXEnabled = sfxOn;

    if (sfxSource != null)
    {
        float db = Mathf.Log10(Mathf.Clamp(sfxVolume, 0.0001f, 1f)) * 20f;
        sfxSource.outputAudioMixerGroup.audioMixer.SetFloat("SFXVolume", db);
    }
}


    // Convenience methods (optional)
    public void PlayJumpSound() => PlaySFX(jumpClip);
    public void PlayShootSound() => PlaySFX(shootClip);
    public void PlaySpawnSound() => PlaySFX(spawnClip);
    public void PlayDeathSound() => PlaySFX(deathClip);
    public void PlayCoinSound() => PlaySFX(coinClip);
}
