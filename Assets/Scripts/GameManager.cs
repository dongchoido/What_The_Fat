using UnityEngine;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject[] players;
    public string playerTag = "Player";

    [Header("References")]
    public GameObject playerPrefab;

    [Header("UI")]
    public int currentGold = 0;
    public TextMeshProUGUI goldText;

    [Header("Jump Settings")]
    public float jumpDelayPerPlayer = 0.1f;

    [Header("Line Settings")]
    public float leaderX = -2f;
    public float minX = -7.5f;
    public float defaultSpacing = 1.5f;
    public float spacing = 1.5f;

    [Header("Ground")]
    public float scrollSpeed = 4f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        RefreshPlayers();
        UpdateGoldUI();
    }

    void Update()
    {
        if (players == null || players.Length == 0) return;

        PlayerMovement leader = players[0].GetComponent<PlayerMovement>();
        if (leader == null) return;

        if (Input.GetKeyDown(KeyCode.Space) && leader.IsGrounded())
        {
            leader.StartJump();
            BroadcastJump(leader);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            leader.StopJump();
            BroadcastFall(leader);
        }
        caculateJumpDelay();
    }

    void FixedUpdate()
    {
         RepositionPlayers();
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
        for (int i = 1; i < players.Length; i++)
        {
            var pm = players[i].GetComponent<PlayerMovement>();
            pm.followDelay = i * jumpDelayPerPlayer;
            pm.TriggerJumpFromLeader(leader);
        }
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
            goldText.text = "Gold: " + currentGold;
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
            ? players[players.Length - 1].transform.position + new Vector3(-spacing, 5, 0)
            : new Vector3(leaderX, 0, 0);

        GameObject newPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        newPlayer.tag = playerTag;

        RefreshPlayers();
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

    Destroy(player);
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

            players[i].transform.position = Vector3.Lerp(current, target, Time.deltaTime * 5f);
        }
    }

    private void caculateJumpDelay()
    {
        jumpDelayPerPlayer = spacing/scrollSpeed - 0.01f;
    }
}
