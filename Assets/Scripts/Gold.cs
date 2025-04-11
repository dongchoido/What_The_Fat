using UnityEngine;

public class Gold : MonoBehaviour
{
    public int goldValue = 1; // Số lượng vàng khi nhặt

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("GOld");
            GameManager.Instance.AddGold(goldValue); // Cộng vàng vào GameManager
            Destroy(gameObject); // Xoá đối tượng vàng
        }
    }
}
