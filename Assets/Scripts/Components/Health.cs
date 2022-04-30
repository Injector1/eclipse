using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public float MaxHealth = 100;
    [SerializeField] public Text alienScore;
    [SerializeField] public Text stationScore;
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
            switch (gameObject.tag)
            {
                case "Enemy":
                    alienScore.text = $"{int.Parse(alienScore.text.Split('/')[0]) + 1}/3";
                    break;
                case "Station":
                    stationScore.text = $"{int.Parse(stationScore.text.Split('/')[0]) + 1}/3";
                    break;
            }
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