using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button pauseButton;
    public GameObject settingMenuUI;
    public Button settingButton;

    public Toggle bgmToggle;
    public Toggle sfxToggle;
    public Slider bgmSlider;
    public Slider sfxSlider;

    public AudioSource bgmSource; // Audio nhạc nền
    public AudioMixer audioMixer; // Mixer chứa exposed param: SFXVolume

    public static bool isPaused = false;

    private void Start()
    {
        Debug.Log("Pause script Start() called");

        // Gán từ PlayerPrefs
        bgmToggle.isOn = PlayerPrefs.GetInt("BGMEnabled", 1) == 1;
        sfxToggle.isOn = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Gắn listener
        bgmToggle.onValueChanged.AddListener(OnBGMToggleChanged);
        sfxToggle.onValueChanged.AddListener(OnSFXToggleChanged);
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        ApplyAudioSettings();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pauseMenuUI.SetActive(isPaused);
        if (pauseButton != null)
            pauseButton.interactable = !isPaused;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        settingMenuUI.SetActive(false);
        if (pauseButton != null)
            pauseButton.interactable = true;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        if (pauseButton != null)
            pauseButton.interactable = true;
        settingMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        SceneManager.LoadScene("MenuScene");
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        settingMenuUI.SetActive(true);
    }

    // === AUDIO EVENTS ===
    private void OnBGMToggleChanged(bool isOn)
    {
        Debug.Log("BGM Toggle: " + isOn);
        PlayerPrefs.SetInt("BGMEnabled", isOn ? 1 : 0);
        ApplyAudioSettings();
    }

    private void OnSFXToggleChanged(bool isOn)
    {
        Debug.Log("SFX Toggle: " + isOn);
        PlayerPrefs.SetInt("SFXEnabled", isOn ? 1 : 0);
        ApplyAudioSettings();
    }

    private void OnBGMVolumeChanged(float volume)
    {
        Debug.Log("BGM Volume: " + volume);
        PlayerPrefs.SetFloat("BGMVolume", volume);
        ApplyAudioSettings();
    }

    private void OnSFXVolumeChanged(float volume)
    {
        Debug.Log("SFX Volume: " + volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        ApplyAudioSettings();
    }

    // === APPLY AUDIO SETTINGS ===
    private void ApplyAudioSettings()
    {
        // BGM volume trực tiếp
        if (bgmSource != null)
        {
            bgmSource.volume = bgmToggle.isOn ? bgmSlider.value : 0f;
        }

        // SFX qua AudioMixer
        if (audioMixer != null)
        {
            float sfxVolume = sfxToggle.isOn ? sfxSlider.value : 0f;
            float db = Mathf.Log10(Mathf.Clamp(sfxVolume, 0.0001f, 1f)) * 20f;
            audioMixer.SetFloat("SFXVolume", db);
            Debug.Log("SFX Mixer Volume set to dB: " + db);
        }
    }
}
