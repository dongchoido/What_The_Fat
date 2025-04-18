using UnityEngine;

public class LoopingParallax1 : MonoBehaviour
{
    public float parallaxFactor = 0.5f; // < 1: chậm hơn scroll
    private float length;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        float speed = 3 * parallaxFactor;
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Nếu BG đã ra khỏi màn, reset lại vị trí
        if (transform.position.x <= startPos.x - length)
        {
            transform.position += Vector3.right * length * 2f;
        }
    }
}
