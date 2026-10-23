using Unity.Hierarchy;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float airAcceleration = 20f;
    [SerializeField] private float airDeceleration = 8f;
    [SerializeField] private float normalGravity = 3f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector2 attackSize = new Vector2(0.8f, 1.2f);
    [SerializeField] private LayerMask enemyLayer;

    public Animator animator;

    private bool hasAirJumped;
    private bool canAirJump;
    private bool isJumping;

    private GroundCheck groundCheck;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float playerHalfHeight;

    private float horizontalInput;

    private void Awake()
    {
        groundCheck = GetComponent<GroundCheck>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHalfHeight = spriteRenderer.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        Flip(horizontalInput);

        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

        Debug.DrawRay(transform.position, Vector2.down * (playerHalfHeight + 0.2f), Color.red);

        if (Input.GetButtonDown("Jump") && (groundCheck.IsGrounded || canAirJump))
        {
            Jump();
            if (!hasAirJumped)
            {
                hasAirJumped = true;
                canAirJump = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }

    }

    void FixedUpdate()
    {
        animator.SetBool("isGrounded", groundCheck.GetIsGrounded(isJumping));

        if (isJumping && rb.linearVelocity.y <= 0)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }

        // Air Jumping Logic
        if (groundCheck.IsGrounded)
        {
            canAirJump = false;
            hasAirJumped = false;
        } else if (!groundCheck.IsGrounded && !hasAirJumped)
        {
            canAirJump = true;
        }

        if (groundCheck.IsGrounded && !isJumping)
        {
            rb.gravityScale = 0;

            if (horizontalInput == 0f)
            {
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                Vector2 slopeDir = -Vector2.Perpendicular(groundCheck.GroundNormal).normalized;
                rb.linearVelocity = slopeDir * horizontalInput * moveSpeed;
            }
        }
        else
        {
            rb.gravityScale = normalGravity;

            float targetX = horizontalInput * moveSpeed;
            float rate = horizontalInput != 0 ? airAcceleration : airDeceleration;
            float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetX, rate * Time.fixedDeltaTime);

            rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
        }
    }

    private void Flip(float moveInput)
    {
        if (moveInput == 0f) return;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveInput);
        transform.localScale = scale;
    }

    private void Jump()
    {
        isJumping = true;
        animator.SetBool("isJumping", true);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackSize);
    }
}
