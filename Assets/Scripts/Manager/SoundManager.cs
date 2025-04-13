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
        if (clip != null && sfxSource != null)
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

    // Convenience methods (optional)
    public void PlayJumpSound() => PlaySFX(jumpClip);
    public void PlayShootSound() => PlaySFX(shootClip);
    public void PlaySpawnSound() => PlaySFX(spawnClip);
    public void PlayDeathSound() => PlaySFX(deathClip);
    public void PlayCoinSound() => PlaySFX(coinClip);
}
