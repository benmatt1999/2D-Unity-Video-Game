using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector2 attackSize = new Vector2(0.8f, 1.2f);
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private LayerMask enemyLayer;

    private void Awake()
    {
        animator = GetComponent <Animator> ();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
            DealDamage();
        }
    }

    private void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0f, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackSize);
    }

}
