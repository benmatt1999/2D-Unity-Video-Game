using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Ground Check Settings")]
    public LayerMask groundLayer;
    private RaycastHit2D groundHit;
    private bool isGrounded;
    public bool IsGrounded => isGrounded;
    private bool wasGrounded;
    public bool WasGrounded => wasGrounded;

    [SerializeField] private float snapDistance = 0.3f;
    [SerializeField] private float skin = 0.02f;
    [SerializeField] private float maxSlope = 45f;
    private readonly RaycastHit2D[] castResults = new RaycastHit2D[4];
    public Vector2 GroundNormal => groundHit ? groundHit.normal : Vector2.up;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public bool GetIsGrounded(bool isJumping)
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

        return isGrounded;
    }

}
