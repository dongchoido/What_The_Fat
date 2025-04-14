using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject settingsPanel; // Gán panel Settings trong Inspector

    // Gọi khi bấm nút Play
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); // Đổi "GameScene" thành tên scene chính của bạn
    }

    // Gọi khi bấm nút Settings
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    // Gọi khi bấm nút Back trong Settings
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    // Gọi khi bấm nút Quit
    public void QuitGame()
    {
        Debug.Log("Thoát game...");
        Application.Quit();
    }
}
