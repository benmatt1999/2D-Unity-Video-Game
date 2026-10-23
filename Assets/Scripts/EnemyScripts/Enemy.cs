using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected int health;
    protected bool isDead = false;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
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

    protected virtual void Die()
    {
        isDead = true;
        if (anim != null)
        {
            anim.SetTrigger("DieTrigger");
        }
    }
}
