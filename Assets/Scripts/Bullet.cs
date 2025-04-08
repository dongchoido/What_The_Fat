using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;  // Tốc độ bay của đạn
    public int damage = 10;    // Sát thương
    public float lifeTime = 10f; // Thời gian tồn tại tối đa

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D ngay khi khởi tạo
    }

    void OnEnable()
    {
        rb.velocity = Vector2.right * speed; // Đặt vận tốc ngay khi bật đạn lên
        CancelInvoke("DestroyBullet");
        Invoke("DestroyBullet", lifeTime);   // Tự hủy sau thời gian nhất định
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Monster"))
    //     {
    //         BossHealth bossHealth = collision.GetComponent<BossHealth>();
    //         if (bossHealth != null)
    //         {
    //             bossHealth.TakeDamage(damage);
    //         }

    //         DestroyBullet();
    //     }
    // }

    void DestroyBullet()
    {
        gameObject.SetActive(false); // Dùng Object Pooling thay vì Destroy()
    }

    private void OnBecameInvisible()
    {
        DestroyBullet(); // Hủy đạn nếu ra khỏi màn hình để tối ưu hiệu suất
    }
}
