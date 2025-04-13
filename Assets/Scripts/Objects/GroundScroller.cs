using UnityEngine;

public class GroundScroller : MonoBehaviour
{
    public float scrollSpeed = 2f;   // Tốc độ di chuyển mặt đất
    void Start()
    {
        scrollSpeed = GameManager.Instance.scrollSpeed;
    }
    void Update()
    {
        // Di chuyển từ phải sang trái
        transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);
        if (transform.position.x < -200f) // ví dụ: ra khỏi màn hình
        {
            Destroy(gameObject);
        }
    }
}
