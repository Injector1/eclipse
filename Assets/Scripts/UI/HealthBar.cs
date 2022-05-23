using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Health _health;
    private GameObject _healthBar;

    private void Awake()
    {
        _health = GetComponentInParent<Health>();
        _healthBar = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
    }
    
    private void Start()
    {
        _health.OnHealthChange += _ => ChangeBarRotation();
        _health.OnDeath += ChangeBarRotation;
        _healthBar = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
    }

    private void Update()
    {
        if (transform.eulerAngles != Vector3.up)
            transform.eulerAngles = Vector3.up;
    }

    private void ChangeBarRotation()
    {
        _healthBar.transform.localEulerAngles = new Vector3(0, 0, -135 + 90 * _health.currentHealth / _health.maxHealth);
    }
}
