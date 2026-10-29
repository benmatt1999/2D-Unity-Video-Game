using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float invincibilityTime = 3f;
    private float invincibilityTimer;
    private int playerHealth = 3;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
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

        playerHealth -= damage;
        anim.SetTrigger("tookHit");
        invincibilityTimer = invincibilityTime;

        if (playerHealth <= 0)
        {
            PlayerDie();
        } 
    }

    private void PlayerDie()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
