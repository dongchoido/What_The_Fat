using System.Collections;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isLeader = false;
    public Transform targetToFollow;
    public float followDelay = 0.1f;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float ascentSpeed = 3f;
    public float descentSpeed = 5f;
    public float maxJumpHeight = 3f;
    public float gravityScale = 3f;
    public float hoverDuration = 0.5f;
   

    [Header("Ground Check")]
    public Transform groundCheck;
    public float rayDistance = 0.2f;
    public LayerMask groundLayer;
    public bool isGrounded;

    public bool isTouchingMonster;

    private Rigidbody2D rb;

    private bool isHoldingJump = false;
    private bool reachedMaxHeight = false;
    private bool isHovering = false;
    private float startingY;
    private float hoverTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        checkFollowJumping();
    }

    void Update()
    {
        if (!isHoldingJump) 
        {
            CheckGroundRay();
        }
        HandleJumpState();
    }
void OnDrawGizmos()
{
    if (groundCheck != null)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }
}
    void FixedUpdate()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y); 
    }

    void CheckGroundRay()
    {
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        
        if (isGrounded && !isHoldingJump)
        {
            reachedMaxHeight = false;
            isHovering = false;
            rb.gravityScale = gravityScale;
        }
    }

    void HandleJumpState()
    {
        if (!isHoldingJump) return;

        if (!reachedMaxHeight)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, ascentSpeed);

            if (transform.position.y >= startingY + maxJumpHeight)
            {
                reachedMaxHeight = true;
                isHovering = true;
                hoverTimer = hoverDuration;
            }
        }
        else if (isHovering)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            hoverTimer -= Time.deltaTime;

            if (hoverTimer <= 0f)
            {
                isHovering = false;
                rb.gravityScale = gravityScale;
            }
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    public void StartJump()
    {
        if (!isGrounded) return;

        startingY = transform.position.y;
        isHoldingJump = true;
        reachedMaxHeight = false;
        isHovering = false;
        rb.gravityScale = 0f;
        isGrounded = false;
        SoundManager.Instance.PlayJumpSound();
    }

    public void StopJump()
    {
        if (!isHoldingJump) return;

        isHoldingJump = false;
        isHovering = false;
        rb.gravityScale = gravityScale;
    }

    public void InheritJumpState(PlayerMovement leader)
    {
        startingY = transform.position.y;
        isHoldingJump = true;
        reachedMaxHeight = false;
        isHovering = false;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(0f, leader.ascentSpeed);
    }

    public void TriggerJumpFromLeader(PlayerMovement leader)
    {
        StartCoroutine(DelayedInheritJump(leader, followDelay));
    }

    public void TriggerFallFromLeader(PlayerMovement leader)
    {
        StartCoroutine(DelayedStopJump(followDelay));
    }

    IEnumerator DelayedInheritJump(PlayerMovement leader, float delay)
    {
        yield return new WaitForSeconds(delay);
        InheritJumpState(leader);
    }

    IEnumerator DelayedStopJump(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopJump();
    }

    public bool IsGrounded() => isGrounded;
    public bool IsHoldingJump() => isHoldingJump;

    private void checkFollowJumping()
    {
        GameObject prePlayer = null;
        if(GameManager.Instance.players.Length > 1)
            { prePlayer = GameManager.Instance.players[GameManager.Instance.players.Length -2 ];
                PlayerMovement comp = prePlayer.GetComponent<PlayerMovement>();
        if(comp.IsGrounded())
            {
                followDelay = GameManager.Instance.delay;
                TriggerJumpFromLeader(GameManager.Instance.players[0].GetComponent<PlayerMovement>());
            }
            
       }
    }

    public void goToPosition(Vector3 x,Vector3 y)
    {
        if(!isTouchingMonster)
            transform.position = Vector3.Lerp(x,y,Time.deltaTime * 5f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.CompareTag("Monster"))
            isTouchingMonster = true;
        if(other.collider.CompareTag("Ground"))
                isGrounded=true;
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if(other.collider.CompareTag("Monster"))
            isTouchingMonster = false;
        if(other.collider.CompareTag("Ground"))
            isGrounded=false;
    }
}
