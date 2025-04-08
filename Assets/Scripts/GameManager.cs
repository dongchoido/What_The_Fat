using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject[] players;
    public string playerTag = "Player";
    
    [Header("Position Settings")]
    public float leaderX = -2f;
    public float spacing = 1.5f; // Khoảng cách cố định giữa các nhân vật
    public float minLastPlayerX = -7.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        RefreshPlayers();
    }

    void FindAndOrderPlayers()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag(playerTag);
        players = allPlayers.OrderByDescending(p => p.transform.position.x).ToArray();
        
        for (int i = 0; i < players.Length; i++)
        {
            players[i].name = "Player_" + i;
        }
    }

    void SetupPlayerRoles()
    {
        if (players.Length == 0) return;
        
        players[0].GetComponent<PlayerMovement>().isLeader = true;
        
        for (int i = 1; i < players.Length; i++)
        {
            PlayerMovement pm = players[i].GetComponent<PlayerMovement>();
            pm.isLeader = false;
            pm.targetToFollow = players[i-1].transform;
        }
    }

    void AdjustPlayerPositions()
    {
        if (players.Length == 0) return;

        // Tính toán tổng khoảng cách cần thiết
        float totalSpaceNeeded = (players.Length - 1) * spacing;
        
        // Điều chỉnh vị trí Leader nếu cần
        float adjustedLeaderX = Mathf.Max(leaderX, minLastPlayerX + totalSpaceNeeded);
        
        // Đặt vị trí các nhân vật cách đều
        for (int i = 0; i < players.Length; i++)
        {
            float newX = adjustedLeaderX - (i * spacing);
            players[i].transform.position = new Vector3(
                newX,
                players[i].transform.position.y,
                players[i].transform.position.z
            );
        }
    }

    public void RefreshPlayers()
    {
        FindAndOrderPlayers();
        SetupPlayerRoles();
        AdjustPlayerPositions();
    }
}