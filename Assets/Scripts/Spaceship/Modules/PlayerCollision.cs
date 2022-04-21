using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class PlayerCollision : MonoBehaviour
{
    private Animator animator;
    public HealthBar healthBar;
    public int maxHealth = 10;
    private int currentHealth;

    private void Awake()
    {
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
    }

    void StopAnimations()
    {
        animator.SetBool("isShooting", false);
        animator.SetBool("isMoving", false);
        animator.SetBool("isDead", true);
    }

    async void Death()
    {
        animator = GetComponent<Animator>();
        StopAnimations();
        Time.timeScale = 1f;
        await Task.Delay(1200); //1200 ms to play animation
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionObject = collision.gameObject.tag;

        switch (collisionObject)
        {
            case "Planet":
                TakeDamage(3);
                break;
            case "EnemyBullets":
                TakeDamage(1);
                break;
            case "Asteroids":
                TakeDamage(100);
                break;
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        
        if (currentHealth <= 0)
        {
            healthBar.SetHealth(0);
            Death();
        }
    }
}
