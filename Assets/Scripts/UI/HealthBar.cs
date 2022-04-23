using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider Slider;
    public Gradient Gradient;
    public Image Fill;
    private Health _health;
    private Actions _actions;
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        _health = _player.GetComponent<Health>();
        _actions = _player.GetComponent<Actions>();
        SetMaxHealth(_health.MaxHealth);
        _actions.OnHealthChange += SetHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        Slider.maxValue = maxHealth;
        Slider.value = maxHealth;
        Fill.color = Gradient.Evaluate(Slider.normalizedValue);
    }

    public void SetHealth(float healthChange)
    {
        Slider.value = _health.CurrentHealth;
        Fill.color = Gradient.Evaluate(Slider.normalizedValue);
    }
}
