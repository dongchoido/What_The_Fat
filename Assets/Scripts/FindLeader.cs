using UnityEngine;
using System.Linq; // Dùng để sắp xếp nhân vật theo vị trí X

public class FindLeader : MonoBehaviour
{
    [SerializeField] private float followDelay = 0.2f;
    [SerializeField] private float jumpForce = 5f;
    
    private Transform leader;
    private bool isLeader = false;
    private Rigidbody2D rb;
    
    // Buffer lưu độ cao của Leader trong quá khứ
    private float[] heightBuffer;
    private float[] timeBuffer;
    private int bufferSize;
    private int oldestIndex;
    private int newestIndex;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Tìm tất cả nhân vật có tag "Player" và sắp xếp theo vị trí X (từ trái sang phải)
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player")
                              .OrderBy(player => player.transform.position.x)
                              .ToArray();
        
        // Leader là nhân vật CUỐI CÙNG (vị trí X lớn nhất = ngoài cùng bên phải)
        leader = allPlayers[allPlayers.Length - 1].transform;
        
        // Kiểm tra nếu đây là Leader
        if (transform == leader)
        {
            isLeader = true;
            Debug.Log(gameObject.name + " là Leader (ngoài cùng phải)");
        }
        else
        {
            // Khởi tạo buffer cho Follower
            bufferSize = Mathf.CeilToInt(followDelay / Time.fixedDeltaTime);
            heightBuffer = new float[bufferSize];
            timeBuffer = new float[bufferSize];
            
            float currentHeight = leader.position.y;
            for (int i = 0; i < bufferSize; i++)
            {
                heightBuffer[i] = currentHeight;
                timeBuffer[i] = 0f;
            }
            
            oldestIndex = 0;
            newestIndex = bufferSize - 1;
        }
    }

    void Update()
    {
        // Chỉ Leader mới được nhảy khi ấn Space
        if (isLeader && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        if (!isLeader && leader != null)
        {
            // Cập nhật buffer độ cao của Leader
            newestIndex = (newestIndex + 1) % bufferSize;
            heightBuffer[newestIndex] = leader.position.y;
            timeBuffer[newestIndex] = Time.time;
            
            if (newestIndex == oldestIndex)
            {
                oldestIndex = (oldestIndex + 1) % bufferSize;
            }
            
            // Lấy độ cao từ 0.2 giây trước
            float targetTime = Time.time - followDelay;
            while (oldestIndex != newestIndex && timeBuffer[oldestIndex] < targetTime)
            {
                oldestIndex = (oldestIndex + 1) % bufferSize;
            }
            
            // Áp dụng độ cao vào Follower (mượt hơn với Lerp)
            float targetY = heightBuffer[oldestIndex];
            Vector3 newPos = transform.position;
            newPos.y = Mathf.Lerp(newPos.y, targetY, 0.1f); // Giảm giật cục
            transform.position = newPos;
        }
    }
}