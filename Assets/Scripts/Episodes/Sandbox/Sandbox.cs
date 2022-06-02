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
        _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        engine.MaxSpeedSqr = _sliderMaxSpeed.value * _sliderMaxSpeed.value;
    }
    
    [SerializeField] private Slider _sliderEnginePower;
    public void SetEnginePower()
    {
        var engine = _player.GetComponent<BasicMainEngine>();
        _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        engine.Power = _sliderEnginePower.value * _sliderEnginePower.value / 20000;
    }

    public void ChangeGravityIgnore()
    {
        var gv = _player.GetComponent<GravityVisitor>();
        gv.DontTouchMe = !gv.DontTouchMe;
    }
    
    [SerializeField] private Slider _sliderGunDamage;
    public void SetGunDamage()
    {
        var gun1 = _player.transform.GetChild(0).GetChild(0);
        var gun2 = _player.transform.GetChild(1).GetChild(0);
        gun1.GetComponent<Bullet>().Damage = _sliderGunDamage.value;
        gun2.GetComponent<Bullet>().Damage = _sliderGunDamage.value;
    }
    
    [SerializeField] private Slider _sliderGunCooldown;
    public void SetGunCooldown()
    {
        var gun1 = _player.transform.GetChild(0).GetComponent<CoolDown>().CoolDownInSec = _sliderGunCooldown.value;
        var gun2 = _player.transform.GetChild(1).GetComponent<CoolDown>().CoolDownInSec = _sliderGunCooldown.value;;
    }
    
    [SerializeField] private Slider _sliderBulletSpeed;
    public void SetBulletSpeed()
    {
        var gun1 = _player.transform.GetChild(0).GetComponent<PlasmaGun>().BulletSpeed = _sliderBulletSpeed.value;
        var gun2 = _player.transform.GetChild(1).GetComponent<PlasmaGun>().BulletSpeed = _sliderBulletSpeed.value;;
    }
    
    [SerializeField] private Slider _sliderBulletSpreading;
    public void SetBulletSpreading()
    {
        var gun1 = _player.transform.GetChild(0).GetComponent<PlasmaGun>().Spreading = _sliderBulletSpreading.value;
        var gun2 = _player.transform.GetChild(1).GetComponent<PlasmaGun>().Spreading = _sliderBulletSpreading.value;;
    }

    public void SpawnEnemy()
    {
        var spawner = GameObject.FindWithTag("Utilities").GetComponent<PrefabsSpawner>();
        var randomEx = new RandomExtensions(true);
        spawner.Spawn("Enemy", _player.transform.position + randomEx.GetRandomVector(3, 7), 360 * randomEx.GetFloat());
    }
    
    public void SpawnStation()
    {
        var spawner = GameObject.FindWithTag("Utilities").GetComponent<PrefabsSpawner>();
        var randomEx = new RandomExtensions(true);
        spawner.Spawn("Station", _player.transform.position + randomEx.GetRandomVector(5, 10), 360 * randomEx.GetFloat());
    }
}
