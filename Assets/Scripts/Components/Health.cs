using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public float MaxHealth = 100;
    [SerializeField] public Text score;
    [SerializeField] private AudioSource enemyDeath;
    public float CurrentHealth;
    public Action<float> OnHealthChange;
    public Action OnDeath;
    

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        OnHealthChange += HealthAdd;
        OnDeath += BasicDeath;
    }
    
    async public void BasicDeath()
    {
        score.text = $"{int.Parse(score.text.Split('/')[0]) + 1}/3";
        enemyDeath.Play();
        await Task.Delay(400);
        gameObject.SetActive(false);
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
