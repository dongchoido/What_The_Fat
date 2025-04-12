using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public GameObject[] groundPrefabs;
    public float spawnThreshold = 117f;      // Spawn khi đoạn trước đã cách xa vị trí spawn 100 đơn vị

    private GameObject lastSpawnedGround;    // Ground vừa spawn trước đó

    public float distance;
    void Start()
    {
        SpawnGround();  // Spawn đoạn đầu tiên
    }

    void Update()
    {
        if (lastSpawnedGround == null) return;

         distance = transform.position.x - lastSpawnedGround.transform.position.x;

        if (distance >= spawnThreshold)
        {
            SpawnGround();
        }
    }

    void SpawnGround()
    {
        int index = Random.Range(0, groundPrefabs.Length);
        GameObject newGround = Instantiate(groundPrefabs[index]);

        // Spawn tại vị trí của MapSpawner (đứng yên)
        newGround.transform.position = transform.position;

        // Lưu lại ground cuối cùng đã spawn
        lastSpawnedGround = newGround;

        spawnThreshold = 117+ (GameManager.Instance.scrollSpeed)/2;
    }
}
