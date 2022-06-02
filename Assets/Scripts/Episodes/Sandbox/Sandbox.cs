using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class Sandbox : MonoBehaviour
{
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _changeInvulnerability = ChangeInvulnerability1();
    }
    
    private Action<float> _onHealthChangeBackup;
    private IEnumerator _changeInvulnerability;
    public void ChangeInvulnerability() => _changeInvulnerability.MoveNext();
    private IEnumerator ChangeInvulnerability1()
    {
        var health = _player.GetComponent<Health>();
        _onHealthChangeBackup = health.OnHealthChange;
        while (true)
        {
            health.OnHealthChange = new Action<float>(_ => { });
            Debug.Log("Godmode enabled");
            yield return null;
            health.OnHealthChange = _onHealthChangeBackup;
            Debug.Log("Godmode disabled");
            yield return null;
        }
    }

    public void RegenHealth()
    {
        var health = _player.GetComponent<Health>();
        health.CurrentHealth = health.MaxHealth;
        var shield = _player.GetComponent<Shield>();
        shield.CurrentShield = shield.MaxShield;
        shield.OnShieldChange();
        health.OnHealthChange(0);
    }

    [SerializeField] private Slider _sliderMaxSpeed;
    public void SetEngineMaxSpeed()
    {
        var engine = _player.GetComponent<BasicMainEngine>();
        engine.MaxSpeedSqr = _sliderMaxSpeed.value * _sliderMaxSpeed.value;
    }
}
