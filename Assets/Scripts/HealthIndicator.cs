using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour
{
    public Slider slider;

    public void InitializeHealthBar(int maxHealth)
    {
        slider.maxValue = maxHealth;
        SetHealth(maxHealth);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
