using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float destroyDelay = 0.1f;
    private bool isTriggered = false; // Ngăn chặn trigger nhiều lần

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;
        
        if (other.CompareTag("Player"))
        {
            isTriggered = true;
            GameManager.Instance.RemovePlayer(other.gameObject);
            Destroy(other.gameObject);
            Destroy(gameObject, destroyDelay);
        }
    }
}