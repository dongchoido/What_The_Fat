using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI catHistoryText;
    public TextMeshProUGUI catRecordText;
    public Button retryButton;
    public Button menuButton;

    void Start()
    {
        // Gán sự kiện cho nút
        retryButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(ReturnToMenu);

        gameObject.SetActive(false); // Ẩn ban đầu
    }

    public void gameOver()
    {
        coinText.text = "Coins: " + GameManager.Instance.currentGold.ToString();
        catHistoryText.text = "Cats: " + GameManager.Instance.catHistory.ToString();
        catRecordText.text = "Record: " + PlayerPrefs.GetInt("HighScore").ToString();
        gameObject.SetActive(true);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale =1;
    }

    void ReturnToMenu()
    {
          Time.timeScale =1;
        SceneManager.LoadScene("MenuScene"); // Đảm bảo bạn có scene tên này
      
    }
}
