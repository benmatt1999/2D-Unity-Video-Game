using UnityEngine;

public class Bat : Enemy
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D rb;
    private Transform player;
    private float skin = 0.01f;
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }
    void FixedUpdate()
    {
        if (isDead || player == null) return;

        Vector2 currentPos = rb.position;
        Vector2 newPos = Vector2.MoveTowards(currentPos, player.position, speed * Time.fixedDeltaTime);
        Vector2 move = newPos - currentPos;
        rb.MovePosition(GetSafePosition(currentPos, move));

        float dx = currentPos.x - player.position.x;
        if (Mathf.Abs(dx) > 0.1f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(dx);
            transform.localScale = scale;
        }

    }

    private Vector2 GetSafePosition(Vector2 currentPos, Vector2 move)
    {
        float distance = move.magnitude;
        if (distance < 0.0001f) return currentPos;

        Vector2 direction = move / distance;
        float radius = Mathf.Min(hitbox.bounds.extents.x, hitbox.bounds.extents.y);
        Vector2 offset = (Vector2)hitbox.bounds.center - currentPos;

        RaycastHit2D hit = Physics2D.CircleCast(currentPos + offset, radius, direction, distance, groundLayer);
        if (!hit || hit.distance <= 0f)
        {
            return currentPos + move;
        }

        Vector2 contactPos = currentPos + direction * Mathf.Max(0f, hit.distance - skin);

        Vector2 tangent = Vector2.Perpendicular(hit.normal);
        Vector2 toPlayer = (Vector2)player.position - currentPos;
        if (Vector2.Dot(toPlayer, tangent) < 0f)
        {
            tangent = -tangent;
        }

        Vector2 slide = tangent * distance;

        RaycastHit2D slideHit = Physics2D.CircleCast(contactPos + offset, radius, tangent, distance, groundLayer);
        if (!slideHit)
        {
            return contactPos + slide;
        }

        return contactPos + tangent * Mathf.Max(0f, slideHit.distance - skin);
    }

    private void OnDrawGizmos()
    {
        Collider2D col = hitbox != null ? hitbox : GetComponent<Collider2D>();
        if (col == null) return;

        float radius = Mathf.Min(col.bounds.extents.x, col.bounds.extents.y);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(col.bounds.center, radius);
    }
}
