using Unity.Hierarchy;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float maxSlope = 45f;
    [SerializeField] private float normalGravity = 3f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector2 attackSize = new Vector2(1f, 1.2f);
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float snapDistance = 0.3f;
    [SerializeField] private float skin = 0.02f;

    public Animator animator;

    [Header("Ground Check Settings")]
    public LayerMask groundLayer;
    private RaycastHit2D groundHit;
    private bool isGrounded;
    private bool wasGrounded;
    private bool hasAirJumped;
    private bool canAirJump;

    private bool isJumping;
    private readonly RaycastHit2D[] castResults = new RaycastHit2D[4];

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float playerHalfHeight;

    private float horizontalInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

        if (Input.GetButtonDown("Jump") && (GetIsGrounded() || canAirJump))
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
        GetIsGrounded();

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        if (isJumping && rb.linearVelocity.y <= 0)
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }

        // Air Jumping Logic
        if (isGrounded)
        {
            canAirJump = false;
            hasAirJumped = false;
        } else if (!isGrounded && !hasAirJumped)
        {
            canAirJump = true;
        }

        if (isGrounded && !isJumping)
        {
            rb.gravityScale = 0;

            if (horizontalInput == 0f)
            {
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                Vector2 slopeDir = -Vector2.Perpendicular(groundHit.normal).normalized;
                rb.linearVelocity = slopeDir * horizontalInput * moveSpeed;
            }
        }
        else
        {
            rb.gravityScale = normalGravity;
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    private bool GetIsGrounded()
    {
        wasGrounded = isGrounded;

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(groundLayer);

        int count = rb.Cast(Vector2.down, filter, castResults, snapDistance);

        groundHit = default;
        for (int i = 0; i < count; i++)
        {
            RaycastHit2D h = castResults[i];
            if (Vector2.Angle(h.normal, Vector2.up) > maxSlope) continue;
            if (!groundHit || h.distance < groundHit.distance) groundHit = h;
        }

        bool touching = groundHit && groundHit.distance <= skin;
        bool canSnap = groundHit && wasGrounded && !isJumping;

        isGrounded = touching || canSnap;

        if (canSnap && !touching)
        {
            rb.position += Vector2.down * (groundHit.distance - skin * 0.5f);
        }

        animator.SetBool("isGrounded", isGrounded);

        return isGrounded;
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
