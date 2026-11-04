using Unity.VisualScripting;
using UnityEngine;

public class Zombie : Enemy
{
    [Header("PitJumping")]
    [SerializeField] private float edgeCheckDist = 0.1f;
    [SerializeField] private float edgeCheckDepth = 1.0f;
    [SerializeField] private float landingCheckDepth = 2.0f;
    [SerializeField] private float jumpUpVelocity = 7f;
    [SerializeField] private float jumpAcrossVelocity = 3.5f;

    private GroundCheck groundCheck;
    private Collider2D collider;
    private bool isJumping = false;

    protected override void Awake()
    {
        base.Awake();
        groundCheck = GetComponent<GroundCheck>();
        collider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (isDead || player == null)
        {
            return;
        }

        if (isJumping && rb.linearVelocity.y <= 0f && groundCheck.IsGrounded)
        {
            isJumping = false;
        }

        anim.SetBool("isGrounded", groundCheck.GetIsGrounded(isJumping));
        float dx = player.position.x - rb.position.x;
        float direction = Mathf.Abs(dx) > 0.1f ? Mathf.Sign(dx) : 0f;
        FaceTowards(-dx);

        if (groundCheck.IsGrounded && !isJumping && !AtEdge(direction))
        {
            anim.SetTrigger("MoveTrigger");
            rb.gravityScale = 0;
            Vector2 slopeDir = -Vector2.Perpendicular(groundCheck.GroundNormal).normalized;

            rb.linearVelocity = slopeDir * direction * speed;
        } 
        else if (groundCheck.IsGrounded && !isJumping && AtEdge(direction))
        {
            Debug.DrawRay(rb.position, groundCheck.GroundNormal * 1.5f, Color.red);

            if (CanLandAcross(direction))
            {
                Jump(direction);
                return;
            }
        }
    }

    private Vector2 FeetPosition => new Vector2(collider.bounds.center.x, collider.bounds.min.y + 0.05f);

    private bool AtEdge(float direction)
    {
        Vector2 posCheckOrigin = FeetPosition + Vector2.right * direction * (collider.bounds.extents.x + edgeCheckDist);
        return !Physics2D.Raycast(posCheckOrigin, Vector2.down, edgeCheckDepth, groundLayer);
    }

    private bool CanLandAcross(float direction)
    {
        Vector2 posCheckOrigin = FeetPosition + Vector2.right * direction * (collider.bounds.extents.x + edgeCheckDist + landingCheckDepth);
        return Physics2D.Raycast(posCheckOrigin, Vector2.down, edgeCheckDepth, groundLayer);
    }

    private void Jump(float direction)
    {
        isJumping = true;
        rb.gravityScale = normalGravity;
        rb.linearVelocity = new Vector2(direction * jumpAcrossVelocity, jumpUpVelocity);
        anim.SetTrigger("JumpTrigger");
    }
}
