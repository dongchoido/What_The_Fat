using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Spawn Settings")]
    public float spawnInterval = 2f; // Thời gian giữa các lần spawn
    public Transform spawnPoint;     // Vị trí spawn (hoặc để trống để spawn tại vị trí của object này)

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnFromPool();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnFromPool()
    {
        GameObject obj = BulletPool.Instance.GetBullet();
        if (obj != null)
        {
            obj.transform.position = spawnPoint ? spawnPoint.position : transform.position;
            obj.transform.rotation = Quaternion.identity;
            obj.SetActive(true);
        }
    }
}
