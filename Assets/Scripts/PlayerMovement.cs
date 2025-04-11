using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public bool isLeader = false;
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float ascentSpeed = 3f;
    public float descentSpeed = 5f;
    public float maxJumpHeight = 3f;
    public float gravityScale = 3f;
    public float hoverDuration = 0.5f;
    public float jumpBufferTime = 0.2f;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public float fireRate = 0.5f;
    public Transform firePoint;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    private List<Collider2D> groundContacts = new List<Collider2D>();

    [Header("Follow Settings")]
    public float followDelay = 0.2f;
    public float followSmoothTime = 0.1f;
    private Vector2 followVelocity;

    public Rigidbody2D rb;
    public bool isGrounded;
    public bool isHoldingJump;
    public bool reachedMaxHeight;
    public bool isHovering;
    public bool mustReleaseSpace;
    public float startingY;
    public float hoverTimer;
    private float nextFireTime;
    private float jumpBufferTimer;

    public Transform targetToFollow;
    private Queue<Vector3> positionHistory;
    private Queue<float> timeHistory;

    public bool isLocked = false; // Ngăn điều khiển khi reposition

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
        if (isLocked) return;

        UpdateGroundStatus();
        HandleShooting();
        HandleJumpBuffer();

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

    void HandleJumpBuffer()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded)
        {
            jumpBufferTimer = jumpBufferTime;
        }
        else if (jumpBufferTimer > 0)
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if (isGrounded && jumpBufferTimer > 0 && !mustReleaseSpace)
        {
            StartAscent();
            jumpBufferTimer = 0f;
        }
    }

    void UpdateGroundStatus()
    {
        bool wasGrounded = isGrounded;
        isGrounded = groundContacts.Count > 0;

        if (!wasGrounded && isGrounded)
        {
            nextFireTime = Time.time + fireRate;
        }
    }

    void HandleShooting()
    {
        if (isGrounded && Time.time >= nextFireTime)
        {
            GameObject bullet = BulletPool.Instance.GetBullet();
            if (bullet != null)
            {
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = Quaternion.identity;
                bullet.SetActive(true);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void HandleLeaderInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !mustReleaseSpace)
        {
            StartAscent();
        }

        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            mustReleaseSpace = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            mustReleaseSpace = false;
            StopAscent();
        }
    }

    void FixedUpdate()
    {
        if (isLocked) return;

        if (isLeader)
        {
            HandleMovement();
        }
        else if (targetToFollow != null)
        {
            ApplyFollowPosition();
        }
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

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
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }
    }

    public void StartAscent()
    {
        startingY = transform.position.y;
        isHoldingJump = true;
        reachedMaxHeight = false;
        isHovering = false;
        mustReleaseSpace = false;
        rb.gravityScale = 0f;
    }

    public void StopAscent()
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            if (!groundContacts.Contains(collision.collider))
            {
                groundContacts.Add(collision.collider);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            groundContacts.Remove(collision.collider);
        }
    }

    void OnDestroy()
    {
        if (isLeader && GameManager.Instance != null)
        {
            GameManager.Instance.RemovePlayer(gameObject);
        }
    }

    void UpdateFollowPosition()
    {
        if (targetToFollow != null)
        {
            positionHistory.Enqueue(targetToFollow.position);
            timeHistory.Enqueue(Time.time);

            while (timeHistory.Count > 0 && timeHistory.Peek() < Time.time - followDelay)
            {
                positionHistory.Dequeue();
                timeHistory.Dequeue();
            }
        }
    }

    void ApplyFollowPosition()
    {
        if (positionHistory.Count > 0 && targetToFollow != null)
        {
            Vector3 targetPosition = positionHistory.Peek();
            Vector3 newPosition = Vector2.SmoothDamp(
                transform.position,
                new Vector2(transform.position.x, targetPosition.y),
                ref followVelocity,
                followSmoothTime
            );
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
    }

    public void InheritJumpState(PlayerMovement previousLeader)
    {
        isHoldingJump = previousLeader.isHoldingJump;
        reachedMaxHeight = previousLeader.reachedMaxHeight;
        isHovering = previousLeader.isHovering;
        mustReleaseSpace = previousLeader.mustReleaseSpace;
        startingY = previousLeader.startingY;
        hoverTimer = previousLeader.hoverTimer;
        rb.gravityScale = previousLeader.rb.gravityScale;
    }
}
