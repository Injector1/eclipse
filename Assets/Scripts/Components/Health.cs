using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;


public class Health : MonoBehaviour
{
    [SerializeField] public float MaxHealth = 100;

    public float CurrentHealth;
    public Action<float> OnHealthChange;
    public Action OnDeath;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        OnHealthChange += HealthAdd;
        OnDeath += Death;
    }

    //TODO перенести в interesting game manager
    void Death()
    {   
        if (!gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            return;
        }
        EventPlanner.PostponeAnEvent(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name), 1200);
    }
    
    private void HealthAdd(float hpAdd)
    {
        CurrentHealth += hpAdd;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnHealthChange = null;
            OnDeath?.Invoke();
            return;
        }
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }
}
