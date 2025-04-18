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
    public Transform forwardCheck;
    public float rayDistance = 0.2f;
    public LayerMask groundLayer;
    public LayerMask monsterLayer;
    public bool isGrounded;
    public bool isTouchingMonster;

    private Rigidbody2D rb;

    public bool isHoldingJump = false;
    private bool reachedMaxHeight = false;
    private bool isHovering = false;
    private float startingY;
    private float hoverTimer = 0f;

    [Header("Spawn effect")]
    private SpriteRenderer spriteRenderer;
    private float flashDuration = 0.5f;
    private float flashInterval = 0.1f;
    private float flashTimer = 0f;

    public Animator anim;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        //checkFollowJumping();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Bắt đầu đổi màu mỗi 0.1s
        InvokeRepeating(nameof(FlashColor), 0f, flashInterval);
    }

    void Update()
    {
        CheckGroundRay();
        HandleJumpState();
    }
void OnDrawGizmos()
{
    if (groundCheck != null)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, rayDistance);
    }
    if (forwardCheck != null)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(forwardCheck.position, rayDistance);
    }
}
    void FixedUpdate()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y); 
    }

    void CheckGroundRay()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, rayDistance, groundLayer);
        isTouchingMonster = Physics2D.OverlapCircle(forwardCheck.position, rayDistance, monsterLayer) ||
                            Physics2D.OverlapCircle(forwardCheck.position, rayDistance, groundLayer);

        anim.SetBool("isGrounded",isGrounded);
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

    // private void checkFollowJumping()
    // {
    //     GameObject prePlayer = null;
    //     if(GameManager.Instance.players.Length > 1)
    //         { prePlayer = GameManager.Instance.players[GameManager.Instance.players.Length -2 ];
    //             PlayerMovement comp = prePlayer.GetComponent<PlayerMovement>();
    //     if(comp.IsGrounded())
    //         {
    //             followDelay = GameManager.Instance.delay;
    //             TriggerJumpFromLeader(GameManager.Instance.players[0].GetComponent<PlayerMovement>());
    //         }
            
    //    }
    // }

    public void goToPosition(Vector3 x,Vector3 y)
    {
        if(!isTouchingMonster)
            transform.position = Vector3.Lerp(x,y,Time.deltaTime * 5f);
    }

    void FlashColor()
    {
        // Tăng thời gian đã trôi qua
        flashTimer += flashInterval;

        // Tạo màu ngẫu nhiên bằng RGB
        float r = Random.Range(0.5f, 1f);
        float g = Random.Range(0.5f, 1f);
        float b = Random.Range(0.5f, 1f);
        spriteRenderer.color = new Color(r, g, b);

        // Dừng sau 0.5s và đặt lại màu trắng
        if (flashTimer >= flashDuration)
        {
            CancelInvoke(nameof(FlashColor));
            spriteRenderer.color = Color.white;
        }
    }
  
}
