using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class Health : MonoBehaviour
{
    [SerializeField] public float MaxHealth = 100;
    private Spaceship spaceship;
    public float CurrentHealth;
    public Action<float> OnHealthChange;
    public Action OnDeath;

    private void Awake()
    {
        spaceship = GetComponent<Spaceship>();
        CurrentHealth = MaxHealth;
        OnHealthChange += HealthAdd;
        OnDeath += Death;
    }
    

    async void Death()
    {   
        if (!gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            return;
        }
        Time.timeScale = 1f;
        EventPlanner.PostponeAnEvent(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name), 1200);
    }
    
    private void HealthAdd(float hpAdd)
    {
        CurrentHealth += hpAdd;
        if (CurrentHealth <= 1e-3)
        {
            OnDeath?.Invoke();
            return;
        }
        CurrentHealth %= MaxHealth;
    }
}
