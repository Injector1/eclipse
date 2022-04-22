using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private Health health;

    private void Awake()
    {
        health = GameObject.FindWithTag("Player").GetComponent<Health>();
    }

    private void Start()
    {
        SetMaxHealth(health.MaxHealth);
        health.OnHealthChange += SetHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetHealth(float healthChange)
    {
        slider.value = health.CurrentHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
