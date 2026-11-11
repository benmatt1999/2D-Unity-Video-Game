using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float invincibilityTime = 3f;
    [SerializeField] private int maxHealth = 3;
    private float invincibilityTimer;
    private int currentHealth = 3;
    private Animator anim;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public event System.Action OnHealthChanged;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (invincibilityTimer > 0f)
        {
            invincibilityTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        if (invincibilityTimer > 0f) return;

        Debug.Log("TakeDamage called");

        currentHealth -= damage;
        anim.SetTrigger("tookHit");
        invincibilityTimer = invincibilityTime;
        OnHealthChanged?.Invoke();

        if (currentHealth <= 0)
        {
            PlayerDie();
        } 
    }

    private void PlayerDie()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
