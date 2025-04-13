using UnityEngine;

public class Gold : MonoBehaviour
{
    public int goldValue = 1; // Số lượng vàng khi nhặt

    void FixedUpdate()
    {
        transform.Rotate(new Vector3 (0,5,0));
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddGold(goldValue); // Cộng vàng vào GameManager
            Destroy(gameObject); // Xoá đối tượng vàng
        }
    }
}
