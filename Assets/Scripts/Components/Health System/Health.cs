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
    
    public void BasicDeath()
    {
        if (TryGetComponent(typeof(IController), out var controller))
            ((IController) controller).IsDisabled = true;
        
        ActionPlanner.PostponeAnAction(() => gameObject.SetActive(false), 1000);
    }
    
    private void HealthAdd(float hpAdd)
    {
        if (hpAdd < 0)
            TakeDamage(-hpAdd);
        
        else if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    private void TakeDamage(float damage)
    {
        if (_shield != null)
            damage = _shield.TakeDamage(damage);
        CurrentHealth -= damage;
        if (CurrentHealth > 0) return;
        
        CurrentHealth = 0;
        OnHealthChange = null;
        OnDeath?.Invoke();
    }
}
