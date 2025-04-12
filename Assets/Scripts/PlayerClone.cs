using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            triggered = true;

            GameManager.Instance.AddNewPlayer();

            Destroy(gameObject); // Object chỉ dùng 1 lần
        }
    }
}
