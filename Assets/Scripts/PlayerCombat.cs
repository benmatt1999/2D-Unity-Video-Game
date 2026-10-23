using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector2 attackSize = new Vector2(0.8f, 1.2f);
    [SerializeField] private LayerMask enemyLayer;

    private void Awake()
    {
        animator = GetComponent <Animator> ();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackSize);
    }

}
