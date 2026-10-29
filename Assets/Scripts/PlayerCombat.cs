using UnityEngine;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint1;
    [SerializeField] private Vector2 attackSize1 = new Vector2(0.8f, 1.2f);
    [SerializeField] private Transform attackPoint2;
    [SerializeField] private Vector2 attackSize2 = new Vector2(1.2f, 0.4f);
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private LayerMask enemyLayer;

    private void Awake()
    {
        animator = GetComponent<Animator> ();
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
        HashSet<Enemy> hitEnemies = new HashSet<Enemy>();

        AddHits(attackPoint1, attackSize1, hitEnemies);
        AddHits(attackPoint2, attackSize2, hitEnemies);
        
        foreach (Enemy enemy in hitEnemies)
        {
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    }

    private void AddHits(Transform attackPoint, Vector2 attackSize, HashSet<Enemy> hitEnemies)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0f, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                hitEnemies.Add(enemy);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint1 == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint1.position, attackSize1);

        if (attackPoint2 == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint2.position, attackSize2);
    }

}
