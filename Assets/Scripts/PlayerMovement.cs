using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [Header("Jump Settings")]
    public bool isLeader = false;
    public float ascentSpeed = 3f;
    public float descentSpeed = 5f;
    public float maxJumpHeight = 3f;
    public float gravityScale = 3f;
    public float hoverDuration = 0.5f;
    
    [Header("Follow Settings")]
    public float followDelay = 0.2f;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isHoldingJump;
    private bool reachedMaxHeight;
    private bool isHovering;
    private float startingY;
    private float hoverTimer;
    
    public Transform targetToFollow;
    private Queue<Vector3> positionHistory;
    private Queue<float> timeHistory;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        
        if (!isLeader)
        {
            positionHistory = new Queue<Vector3>();
            timeHistory = new Queue<float>();
        }
    }

    void Update()
    {
        if (isLeader)
        {
            HandleLeaderInput();
            CheckMaxHeight();
            HandleHovering();
        }
        else if (targetToFollow != null)
        {
            UpdateFollowPosition();
        }
    }

    void FixedUpdate()
    {
        if (isLeader)
        {
            HandleVerticalMovement();
        }
        else if (targetToFollow != null)
        {
            ApplyFollowPosition();
        }
    }

    void HandleLeaderInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            StartAscent();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopAscent();
        }
    }

    void HandleVerticalMovement()
    {
        if (isHoldingJump)
        {
            if (!reachedMaxHeight)
            {
                rb.velocity = new Vector2(rb.velocity.x, ascentSpeed);
            }
            else if (!isHovering)
            {
                rb.velocity = new Vector2(rb.velocity.x, -descentSpeed);
            }
            else
            {
                rb.velocity = Vector2.zero; // Dừng lại khi đang hover
            }
        }
    }

    void HandleHovering()
    {
        if (isHovering)
        {
            hoverTimer -= Time.deltaTime;
            if (hoverTimer <= 0f)
            {
                isHovering = false;
            }
        }
    }

    void StartAscent()
    {
        startingY = transform.position.y;
        isHoldingJump = true;
        reachedMaxHeight = false;
        isHovering = false;
        rb.gravityScale = 0f;
    }

    void StopAscent()
    {
        isHoldingJump = false;
        isHovering = false;
        rb.gravityScale = gravityScale;
    }

    void CheckMaxHeight()
    {
        if (isHoldingJump && transform.position.y >= startingY + maxJumpHeight && !reachedMaxHeight)
        {
            reachedMaxHeight = true;
            isHovering = true;
            hoverTimer = hoverDuration;
        }
        
        if (isGrounded)
        {
            reachedMaxHeight = false;
            isHovering = false;
        }
    }

    // ... (Giữ nguyên các hàm Follow và Collision)
    void UpdateFollowPosition()
    {
        positionHistory.Enqueue(targetToFollow.position);
        timeHistory.Enqueue(Time.time);
        
        while (timeHistory.Count > 0 && timeHistory.Peek() < Time.time - followDelay)
        {
            positionHistory.Dequeue();
            timeHistory.Dequeue();
        }
    }

    void ApplyFollowPosition()
    {
        if (positionHistory.Count > 0)
        {
            Vector3 targetPosition = positionHistory.Peek();
            transform.position = new Vector3(
                transform.position.x,
                targetPosition.y,
                transform.position.z
            );
            isGrounded = targetToFollow.GetComponent<PlayerMovement>().isGrounded;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && isLeader)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && isLeader)
        {
            isGrounded = false;
        }
    }
}