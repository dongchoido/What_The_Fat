using UnityEngine;
using System.Linq;
using TMPro;
using System.Linq.Expressions;
using UnityEngine.SocialPlatforms;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject[] players;
    public string playerTag = "Player";

    [Header("References")]
    public GameObject playerPrefab;
    public int catHistory;
    public GameOver gameOver;

    [Header("UI")]
    public int currentGold = 0;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI catText;
    public TextMeshProUGUI catHistoryText;

    [Header("Jump Settings")]
    public float jumpDelayPerPlayer = 0.1f;
    public float delay;


    [Header("Line Settings")]
    public float leaderX = -2f;
    public float minX = -7.5f;
    public float defaultSpacing = 1.5f;
    public float spacing = 1.5f;

    [Header("Ground")]
    public float scrollSpeed = 4f;

    [Header("Magnet Settings")]
    public bool isMagnetActive = false;
    public float magnetTimer = 15f;
    public Transform leaderTarget;
    public float magnetRangeX = 7.5f;
    public GameObject[] coins;
    private bool resetedCoins = false;

    void Awake()
    {
        if (Instance == null) 
        {Instance = this;
      //  DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        RefreshPlayers();
        UpdateGoldUI();
        UpdateCatUI();
        UpdateCoinList();
        catHistory = players.Length;
        UpdateCatHistory();
       // SoundManager.Instance.SyncSettingsFromPrefs();
    }

    void Update()
    {
        if (players == null || players.Length == 0) return;

        PlayerMovement leader = players[0].GetComponent<PlayerMovement>();
        if (leader == null) return;
       bool isJumpPressed = false;
    bool isJumpReleased = false;

    // PC: dùng phím Space
    if (Input.GetKeyDown(KeyCode.Space))
        isJumpPressed = true;
    if (Input.GetKeyUp(KeyCode.Space))
        isJumpReleased = true;

    // Mobile: dùng touch
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
            isJumpPressed = true;
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            isJumpReleased = true;
    }

    // Thực hiện nhảy
    if (isJumpPressed && leader.IsGrounded())
    {
        leader.StartJump();
        BroadcastJump(leader);
    }

    // Thực hiện ngừng nhảy (rơi xuống)
    if (isJumpReleased)
    {
        leader.StopJump();
        BroadcastFall(leader);
    }
        caculateJumpDelay();
    }

    void FixedUpdate()
    {
         RepositionPlayers();
         HandleMagnet();
    }

    public void RefreshPlayers()
    {
        players = GameObject.FindGameObjectsWithTag(playerTag)
            .OrderByDescending(p => p.transform.position.x)
            .ToArray();
        SetupRoles();
    }

    void SetupRoles()
    {
        for (int i = 0; i < players.Length; i++)
        {
            var pm = players[i].GetComponent<PlayerMovement>();
            pm.isLeader = (i == 0);
            pm.targetToFollow = (i > 0) ? players[i - 1].transform : null;
        }
    }

    public void BroadcastJump(PlayerMovement leader)
    {
        int i;
        for (i = 1; i < players.Length; i++)
        {
            var pm = players[i].GetComponent<PlayerMovement>();
            pm.followDelay = i * jumpDelayPerPlayer;
            pm.TriggerJumpFromLeader(leader);
        }
        delay = jumpDelayPerPlayer + 0.02f;
    }

    public void BroadcastFall(PlayerMovement leader)
    {
        for (int i = 1; i < players.Length; i++)
        {
            var pm = players[i].GetComponent<PlayerMovement>();
            pm.followDelay = i * jumpDelayPerPlayer;
            pm.TriggerFallFromLeader(leader);
        }
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateGoldUI();
    }

    void UpdateGoldUI()
    {
        if (goldText != null)
            goldText.text = "" + currentGold;
    }

    void UpdateCatUI()
    {
        if(catText!=null)
            catText.text = "X" + players.Length;
    }

    public void AddNewPlayer()
    {
         spacing = defaultSpacing;
        if (players.Length > 1)
        {
            float totalDistance = leaderX - minX;
            spacing = Mathf.Min(defaultSpacing, totalDistance / (players.Length));
        }

        Vector3 spawnPos = players.Length > 0
            ? players[players.Length - 1].transform.position + new Vector3(-spacing, 0, 0)
            : new Vector3(leaderX, 0, 0);

        GameObject newPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        newPlayer.tag = playerTag;
        SoundManager.Instance.PlaySpawnSound();
        RefreshPlayers();
        UpdateCatUI();
        catHistory++;
        UpdateCatHistory();
    }

    public void RemovePlayer(GameObject player)
{
    // Xóa khỏi mảng players
    players = players.Where(p => p != player).ToArray();
    // Nếu còn player khác → cập nhật leader
    if (players.Length > 0)
    {
        SetupRoles(); // Gán lại leader + targetToFollow
    }
    else if (players.Length == 0)
    {
        HighScoreManager.Instance.AddPlayerCoin(currentGold);
        HighScoreManager.Instance.TrySetNewHighScore(catHistory);
        Debug.Log("Game Over");
        gameOver.gameOver();
        Time.timeScale = 0;
    }
    Destroy(player);
    UpdateCatUI();
}

    void RepositionPlayers()
    {
        if (players.Length == 0) return;

        float spacing = defaultSpacing;
        float totalDistance = leaderX - minX;
        if (players.Length > 1)
            spacing = Mathf.Min(defaultSpacing, totalDistance / (players.Length - 1));

        for (int i = 0; i < players.Length; i++)
        {
            float xPos = Mathf.Max(minX, leaderX - i * spacing);
            Vector3 current = players[i].transform.position;
            Vector3 target = new Vector3(xPos, current.y, current.z);
            PlayerMovement comp = players[i].GetComponent<PlayerMovement>();
            comp.goToPosition(current,target);
        }
    }

    private void caculateJumpDelay()
    {
        jumpDelayPerPlayer = spacing/scrollSpeed - 0.01f;
    }

    public void UpdateCoinList()
{
    coins = GameObject.FindGameObjectsWithTag("Gold")
             .OrderBy(c => c.transform.position.x)
             .ToArray();
}

    private void HandleMagnet()
    {
        if(!isMagnetActive) return;
 
        if(!resetedCoins)
        {
            UpdateCoinList();
            resetedCoins = true;
            magnetTimer=15f;
        }
        if(magnetTimer>0)
        {
            AttractCoins();
            magnetTimer-=Time.deltaTime;
        }
        else
            {isMagnetActive = false;
            coins = new GameObject[0];
            resetedCoins = false;
            magnetTimer=15f;}
    }

    private void AttractCoins()
{
    if (players.Length == 0) return;

    Transform leader = players[0].transform;

    foreach (GameObject coin in coins)
    {
        if (coin == null) continue;

        float coinX = coin.transform.position.x;
        if (coinX < magnetRangeX && coinX > -2f)
        {
            float speed = 20f * Time.deltaTime;
            coin.transform.position = Vector2.MoveTowards(coin.transform.position, leader.position, speed);
        }
    }
}

    public void setMagnetTimer()
    {
        isMagnetActive = true;
        magnetTimer = 15f;       
    }

    void UpdateCatHistory()
{
    catHistoryText.text = "" + catHistory;
}

}