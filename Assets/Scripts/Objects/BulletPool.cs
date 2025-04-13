using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;
    public GameObject bulletPrefab;
    public int poolSize = 20;
    
    private Queue<GameObject> _bulletPool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            _bulletPool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
{
    GameObject bullet;

    if (_bulletPool.Count > 0)
    {
        bullet = _bulletPool.Dequeue();
    }
    else
    {
        bullet = Instantiate(bulletPrefab);
    }

    bullet.SetActive(true); // <- Đầu tiên: bật đạn lên
    bullet.GetComponent<Bullet>().ResetBullet(); // <- Sau đó: reset mọi thứ

    return bullet;
}

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        _bulletPool.Enqueue(bullet);
    }
}