using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    public float lifetime = 2f;
    public int damage = 1;
    
    private Rigidbody2D _rb;
    private float _spawnTime;
    private bool _isActive;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        ResetBullet(); // Gọi reset khi đạn được kích hoạt
    }

    // Hàm reset quan trọng cho Object Pool
    public void ResetBullet()
    {
        _spawnTime = Time.time;
        _isActive = true;
        _rb.velocity = transform.right * speed;
    }

    void Update()
    {
        if (_isActive && Time.time - _spawnTime >= lifetime)
        {
            Deactivate();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isActive) return;

        // // Bỏ qua va chạm với Player và đạn khác
         if (other.CompareTag("Player") || other.CompareTag("Bullet")) return;
        
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<MonsterHealth>()?.TakeDamage(damage);
        }
        Deactivate();
    }

    void Deactivate()
    {
        if (!_isActive) return;
        
        _isActive = false;
        _rb.velocity = Vector2.zero;
        
        if (BulletPool.Instance != null)
        {
            BulletPool.Instance.ReturnBullet(this.gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}