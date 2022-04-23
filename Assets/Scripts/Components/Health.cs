using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public partial class Actions : MonoBehaviour
{
    public Action<float> OnHealthChange;
    public Action OnDeath;
}

public class Health : MonoBehaviour
{
    [SerializeField] public float MaxHealth = 100;
    public float CurrentHealth;
    private Actions _actions;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        _actions = GetComponent<Actions>();
    }

    private void Start()
    {
        _actions.OnHealthChange += HealthAdd;
        _actions.OnDeath += Death;
    }

    //TODO перенести в interesting game manager
    void Death()
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
            _actions.OnDeath?.Invoke();
            return;
        }
        CurrentHealth %= MaxHealth;
    }
}
