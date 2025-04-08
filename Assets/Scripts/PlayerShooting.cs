using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    public float fireRate = 0.5f; // Thời gian giữa các lần bắn
    private float nextFireTime = 0f;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // void Update()
    // {
    //     if (playerMovement.CanJump() && Time.time >= nextFireTime)
    //     {
    //         if (firePoint == null) return; // Ngăn lỗi nếu firePoint không tồn tại

    //         Shoot();
    //         nextFireTime = Time.time + fireRate;
    //     }
    // }

    void Shoot()
    {
        GameObject bullet = BulletPool.instance.GetBullet();
        if (bullet == null) return; // Ngăn lỗi nếu BulletPool không có viên đạn nào

        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.identity;
    }
}
