using UnityEngine;

public class Bat : FlyingEnemy
{
    protected override void Awake()
    {
        base.Awake();
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
