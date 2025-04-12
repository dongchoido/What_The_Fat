using UnityEngine;

public class BackDeadZone : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject sprite;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private bool isExplosive = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.RemovePlayer(collision.gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 1.5f);
    }
}
