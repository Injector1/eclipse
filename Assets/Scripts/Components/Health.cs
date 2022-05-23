using System;
using System.Threading.Tasks;
using UnityEngine;
using static GunModifier;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100;
    public float currentHealth;
    public Action<float> OnHealthChange;
    public Action OnDeath;
    
    [SerializeField] GameObject player;
    private GunModifier _playerGun;
    
    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChange += HealthAdd;
        OnDeath += BasicDeath;
        if (gameObject.CompareTag("Enemy")) _playerGun = player.GetComponent<GunModifier>();
    }
    
    public async void BasicDeath()
    {
        if (gameObject.CompareTag("Enemy")) _playerGun.ImproveGun();
        await Task.Delay(1000);
        gameObject.SetActive(false);
    }
    
    private void HealthAdd(float hpAdd)
    {
        currentHealth += hpAdd;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnHealthChange = null;
            OnDeath?.Invoke();
            return;
        }
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }
}
