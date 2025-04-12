using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject sprite;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private bool isExplosive = true;

    private bool hasExploded = false;
    private bool hasTransformed = false;

    void Update()
    {
        if (hasExploded || hasTransformed) return;

        // TODO: Kiểm tra leader trạng thái đặc biệt => biến vàng
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasExploded) return;

        if (collision.CompareTag("Player"))
        {
            Debug.Log("💥 Bom chạm bất kỳ nhân vật nào → nổ & xóa nhân vật");

            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player == null) return;

            hasExploded = true;

            Explode();

            // Xoá nhân vật chạm bom khỏi game
            GameManager.Instance.RemovePlayer(player.gameObject);
        }
    }

    void Explode()
    {
        if (animator != null)
            animator.SetTrigger("explode");

        if (boxCollider != null)
            boxCollider.enabled = false;

        // Âm thanh/hiệu ứng nổ nếu có

        Invoke(nameof(DestroySelf), 1f);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void TransformToGold()
    {
        hasTransformed = true;
        // GameManager.Instance.AddGold(1);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 1.5f);
    }
}
