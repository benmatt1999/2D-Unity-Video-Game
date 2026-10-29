using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected int health;
    protected Collider2D hitbox;
    protected int enemyDamage = 1;
    protected bool isDead = false;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        hitbox = GetComponent<Collider2D>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        health = health - amount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerStay2D(Collider2D hit)
    {
        PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(enemyDamage);
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        if (hitbox != null)
        {
            hitbox.enabled = false;
        }
        if (anim != null)
        {
            anim.SetTrigger("DeathTrigger");
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
