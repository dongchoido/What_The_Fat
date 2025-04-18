using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject settingsPanel;

    [Header("UI Settings Elements")]
    public Toggle bgmToggle;
    public Toggle sfxToggle;
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        SoundManager.Instance.SyncSettingsFromPrefs(); // Cập nhật âm thanh
        LoadSettingsToUI(); // Cập nhật UI từ PlayerPrefs
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Thoát game...");
        Application.Quit();
    }

    // Gán giá trị từ PlayerPrefs vào UI Toggle và Slider
    private void LoadSettingsToUI()
    {
        if (bgmToggle != null)
            bgmToggle.isOn = PlayerPrefs.GetInt("BGMEnabled", 1) == 1;

        if (sfxToggle != null)
            sfxToggle.isOn = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;

        if (bgmSlider != null)
            bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);

        if (sfxSlider != null)
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
}
