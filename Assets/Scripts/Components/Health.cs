using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        OnDeath += BasicDeath;
    }
    
    public async void BasicDeath()
    {
        if (TryGetComponent(typeof(IController), out var controller))
            ((IController) controller).IsDisabled = true;

        await Task.Delay(1000);
        gameObject.SetActive(false);
    }
    
    private void HealthAdd(float hpAdd)
    {
        if (hpAdd < 0)
            DecreaseHealth(-hpAdd);
        
        else if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    private void DecreaseHealth(float hpDecrease)
    {
        CurrentHealth -= hpDecrease;
        if (CurrentHealth > 0) return;
        
        CurrentHealth = 0;
        OnHealthChange = null;
        OnDeath?.Invoke();
    }
}
