using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public float MaxHealth;
    [SerializeField] public float CurrentHealth;
    public Action<float> OnHealthChange;
    public Action OnDeath;
    private Shield _shield;


    private void Awake()
    {
        _shield = GetComponent<Shield>();
        CurrentHealth = CurrentHealth == 0 ? MaxHealth : CurrentHealth;
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
        if (_shield != null)
            hpDecrease = _shield.TakeDamage(hpDecrease);
        CurrentHealth -= hpDecrease;
        if (CurrentHealth > 0) return;
        
        CurrentHealth = 0;
        OnHealthChange = null;
        OnDeath?.Invoke();
    }
}
