using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    [Header("Settings")]
    public GameObject playerPrefab;
    public float spawnOffsetX = -1.5f; // Âm để sang trái
    public bool destroyAfterClone = true;
    public ParticleSystem cloneEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CloneNewPlayer();
            if (destroyAfterClone) Destroy(gameObject);
        }
    }

    void CloneNewPlayer()
    {
        if (GameManager.Instance.players.Length == 0) return;

        // Lấy player trái nhất (cuối mảng)
        GameObject leftmostPlayer = GameManager.Instance.players[GameManager.Instance.players.Length - 1];
        
        // Tính vị trí spawn (thêm vào bên trái)
        Vector3 spawnPos = new Vector3(
            leftmostPlayer.transform.position.x + spawnOffsetX, // Dùng offset âm
            leftmostPlayer.transform.position.y,
            leftmostPlayer.transform.position.z
        );

        GameObject newPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        GameManager.Instance.AddNewPlayer(newPlayer);

        // Hiệu ứng (nếu có)
        if (cloneEffect != null)
        {
            Instantiate(cloneEffect, spawnPos, Quaternion.identity);
        }
    }
}