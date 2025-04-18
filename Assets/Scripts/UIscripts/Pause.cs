using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button pauseButton;
    public Button settingButton;

    public Toggle bgmToggle;
    public Toggle sfxToggle;

    public static bool isPaused = false;

    private void Start()
    {
        isPaused = false;
        bgmToggle.isOn = PlayerPrefs.GetInt("BGMEnabled", 1) == 1;
        sfxToggle.isOn = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        bgmToggle.onValueChanged.AddListener(OnBGMToggleChanged);
        sfxToggle.onValueChanged.AddListener(OnSFXToggleChanged);

        SoundManager.Instance?.SyncSettingsFromPrefs();
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
        if (pauseButton != null)
            pauseButton.interactable = true;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        if (pauseButton != null)
            pauseButton.interactable = true;
        pauseMenuUI.SetActive(false);
        SceneManager.LoadScene("MenuScene");
    }

    private void OnBGMToggleChanged(bool isOn)
    {
        Debug.Log("BGM Toggle: " + isOn);
        PlayerPrefs.SetInt("BGMEnabled", isOn ? 1 : 0);
        SoundManager.Instance?.SyncSettingsFromPrefs();
    }

    private void OnSFXToggleChanged(bool isOn)
    {
        Debug.Log("SFX Toggle: " + isOn);
        PlayerPrefs.SetInt("SFXEnabled", isOn ? 1 : 0);
        SoundManager.Instance?.SyncSettingsFromPrefs();
    }
}
