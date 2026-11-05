using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected int health;
    [SerializeField] protected int speed;
    [SerializeField] protected LayerMask groundLayer;
    protected Rigidbody2D rb;
    protected Collider2D hitbox;
    protected Vector2 currentPos;
    protected Transform player;

    protected Vector2 newPos;
    protected int enemyDamage = 1;
    protected bool isDead = false;
    protected float normalGravity = 3f;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        hitbox = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("DAMAGE");
        if (isDead) return;
        health = health - amount;
        if (health <= 0)
        {
            Die();
        } else
        {
            EnableStun();
        }
    }

    private void OnTriggerStay2D(Collider2D hit)
    {
        PlayerHealth playerHealth = hit.GetComponentInParent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(enemyDamage);
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        if (hitbox != null)
        {
            hitbox.enabled = false;
        }
        if (anim != null)
        {
            anim.SetTrigger("DeathTrigger");
        }
    }

    private void EnableStun()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        anim.SetTrigger("StunedTrigger");
        hitbox.enabled = false;
    }

    private void DisableStun()
    {
        hitbox.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    protected void FaceTowards(float dx)
    {
        if (Mathf.Abs(dx) > 0.1f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(dx);
            transform.localScale = scale;
        }
    }
}
