using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;
    public GameObject bulletPrefab;
    public int poolSize = 500; // Số viên đạn tối đa trong pool

    private Queue<GameObject> bulletQueue = new Queue<GameObject>();

    void Awake()
    {
        instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletQueue.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        if (bulletQueue.Count > 0)
        {
            GameObject bullet = bulletQueue.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            // Nếu hết viên đạn trong pool, tạo mới (có thể hạn chế nếu cần)
            GameObject bullet = Instantiate(bulletPrefab);
            return bullet;
        }   
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletQueue.Enqueue(bullet);
    }
}
