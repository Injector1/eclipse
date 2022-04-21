using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class Health : MonoBehaviour
{
    public HealthBar healthBar;
    private Spaceship spaceship;
    [SerializeField] public int MaxHealth = 10;
    private int currentHealth;

    private void Awake()
    {
        spaceship = GetComponent<Spaceship>();
        healthBar.SetMaxHealth(MaxHealth);
        currentHealth = MaxHealth;
    }

    private void Start()
    {
        spaceship.OnDeath += Death;
    }

    async void Death()
    {
        Time.timeScale = 1f;
        await Task.Delay(1200); //1200 ms to play animation
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    
    //TODO fix
    void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionObject = collision.gameObject.tag;

        if (collisionObject == "Planet") 
            TakeDamage(3);
        if (collisionObject == "Bullets")
            TakeDamage(1);
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        
        if (currentHealth <= 0)
        {
            healthBar.SetHealth(0);
            spaceship.OnDeath?.Invoke();
        }
    }
}
