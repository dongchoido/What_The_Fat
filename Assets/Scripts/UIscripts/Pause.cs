using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenuUI;      // Gán panel vào đây trong Inspector
    public Button pauseButton;          // Gán nút Pause trong Inspector

    public static bool isPaused = false;

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        pauseMenuUI.SetActive(isPaused);

        // Khóa hoặc mở lại nút Pause
        if (pauseButton != null)
        {
            pauseButton.interactable = !isPaused;
        }

        // Xóa focus khỏi nút Pause để tránh bị kích hoạt lại khi nhấn Space
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);

        // Mở lại nút Pause
        if (pauseButton != null)
        {
            pauseButton.interactable = true;
        }

        // Xóa focus khỏi bất kỳ nút UI nào
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene"); // Đổi thành tên scene menu của bạn
    }

    public void OpenSettings()
    {
        Debug.Log("Open settings panel (sẽ làm sau)");
        // Gắn logic mở bảng cài đặt tại đây
    }
}
