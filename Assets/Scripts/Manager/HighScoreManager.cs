using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance { get; private set; }

    public TextMeshProUGUI highScoreText; // Kéo Text từ UI vào đây
    public int playerCoin;
    public TextMeshProUGUI playerCoinText;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại khi đổi scene
        }
        else
        {
            Destroy(gameObject); // Tránh tạo bản sao
        }
    }

    private void Start()
    {
        // Khởi tạo nếu chưa có
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("PlayerCoin"))
        {
            PlayerPrefs.SetInt("PlayerCoin", 0);
            PlayerPrefs.Save();
        }
        UpdateHighScoreText();
    }

    public void TrySetNewHighScore(int newScore)
    {
        int currentHighScore = PlayerPrefs.GetInt("HighScore");

        if (newScore > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", newScore);
            PlayerPrefs.Save();
            UpdateHighScoreText();
        }
    }

    public void AddPlayerCoin(int coins)
    {
        int currentCoin = PlayerPrefs.GetInt("PlayerCoin");
        PlayerPrefs.SetInt("PlayerCoin",currentCoin + coins);
        PlayerPrefs.Save();
    }

    public void UpdateHighScoreText()
    {
        int highScore = PlayerPrefs.GetInt("HighScore");
        if (highScoreText != null)
        {
            highScoreText.text = "Kỷ lục: " + highScore.ToString();
        }
    }

    public void UpdatePlayerCoinText()
    {
        int coins = PlayerPrefs.GetInt("PlayerCoin");
        if(playerCoinText != null)
        {
            playerCoinText.text = "" + coins;
        }
    }

    // Tùy chọn: Reset thủ công
    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.Save();
        UpdateHighScoreText();
    }
}
