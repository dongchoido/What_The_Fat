using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject[] players;
    public string playerTag = "Player";

    [Header("Position Settings")]
    public float leaderX = -2f;
    public float spacing = 1.5f;
    public float minLastPlayerX = -7.5f;
    public float repositionSpeed = 5f;

    [Header("New Player Settings")]
    public float newPlayerSpawnOffset = 3f;
    public float newPlayerMoveSpeed = 3f;

    [Header("Gold Settings")]
    public int currentGold = 0;
    public TextMeshProUGUI goldText;

    private bool isRepositioning = false;
    private Vector3[] targetPositions;
    private GameObject newPlayerBeingAdded;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        RefreshPlayers();
        UpdateGoldUI();
    }

    void Update()
    {
        if (isRepositioning)
        {
            SmoothReposition();
        }
    }

    public void RemovePlayer(GameObject playerToRemove)
    {
        PlayerMovement removedPlayer = playerToRemove.GetComponent<PlayerMovement>();
        bool wasLeader = removedPlayer.isLeader;

        players = players.Where(p => p != playerToRemove).ToArray();

        if (!wasLeader)
        {
            foreach (var player in players)
            {
                PlayerMovement pm = player.GetComponent<PlayerMovement>();
                if (pm.targetToFollow == playerToRemove.transform)
                {
                    int index = System.Array.IndexOf(players, playerToRemove);
                    if (index > 0) pm.targetToFollow = players[index - 1].transform;
                }
            }
        }

        SetupPlayerRoles(removedPlayer);
        CalculateTargetPositions();
        isRepositioning = true;
    }

    public void AddNewPlayer(GameObject newPlayer)
    {
        if (players.Length == 0)
        {
            newPlayer.GetComponent<PlayerMovement>().isLeader = true;
            players = new GameObject[] { newPlayer };
            return;
        }

        GameObject lastPlayer = players[players.Length - 1];
        Vector3 spawnPos = lastPlayer.transform.position - new Vector3(newPlayerSpawnOffset, 0, 0);

        newPlayer.transform.position = spawnPos;
        players = players.Concat(new GameObject[] { newPlayer }).ToArray();

        newPlayerBeingAdded = newPlayer;
        SetupPlayerRoles();
        CalculateTargetPositions();
        isRepositioning = true;
    }

    void SmoothReposition()
    {
        bool allInPosition = true;

        for (int i = 0; i < players.Length; i++)
        {
            Vector3 targetPos = targetPositions[i];
            targetPos.y = players[i].transform.position.y; // giữ nguyên Y

            float speed = (players[i] == newPlayerBeingAdded) ? newPlayerMoveSpeed : repositionSpeed;

            players[i].transform.position = Vector3.MoveTowards(
                players[i].transform.position,
                targetPos,
                speed * Time.deltaTime
            );

            if (Vector3.Distance(players[i].transform.position, targetPos) > 0.01f)
            {
                allInPosition = false;
            }
        }

        if (allInPosition)
        {
            isRepositioning = false;
            newPlayerBeingAdded = null;

            foreach (var p in players)
            {
                p.GetComponent<PlayerMovement>().isLocked = false;
            }
        }
    }

    void CalculateTargetPositions()
    {
        targetPositions = new Vector3[players.Length];
        float totalSpace = (players.Length - 1) * spacing;
        float adjustedLeaderX = Mathf.Max(leaderX, minLastPlayerX + totalSpace);

        for (int i = 0; i < players.Length; i++)
        {
            targetPositions[i] = new Vector3(
                adjustedLeaderX - (i * spacing),
                players[i].transform.position.y,
                players[i].transform.position.z
            );
        }

        foreach (var p in players)
        {
            p.GetComponent<PlayerMovement>().isLocked = true;
        }
    }

    public void RefreshPlayers()
    {
        players = GameObject.FindGameObjectsWithTag(playerTag)
                   .OrderByDescending(p => p.transform.position.x).ToArray();
        SetupPlayerRoles();
        CalculateTargetPositions();
        isRepositioning = true;
    }

    void SetupPlayerRoles(PlayerMovement previousLeader = null)
    {
        if (players.Length == 0) return;

        for (int i = 0; i < players.Length; i++)
        {
            PlayerMovement pm = players[i].GetComponent<PlayerMovement>();
            bool wasLeader = pm.isLeader;
            pm.isLeader = (i == 0);

            if (i > 0)
            {
                pm.targetToFollow = players[i - 1].transform;
            }
            else
            {
                pm.targetToFollow = null;

                if (!wasLeader && previousLeader != null)
                {
                    pm.InheritJumpState(previousLeader);
                }
            }
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
}
