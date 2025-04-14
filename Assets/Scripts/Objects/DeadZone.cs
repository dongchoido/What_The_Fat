using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject sprite;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private bool isExplosive = true;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if(!player.IsGrounded() || player.isTouchingMonster)
            {
            //Debug.Log("Rơi xuống vực");
 
            CapsuleCollider2D bc = collision.GetComponent<CapsuleCollider2D>();
            bc.isTrigger = true;
            if (player == null) return;
            GameManager.Instance.RemovePlayer(player.gameObject);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 1.5f);
    }
}
