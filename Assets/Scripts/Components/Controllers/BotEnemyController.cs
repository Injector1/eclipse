using System;
using UnityEditor;
using UnityEngine;


public class BotEnemyController : MonoBehaviour, IController
{
    private Engine _engine;
    private Health _health;
    private WeaponController _weaponController;
    private Rigidbody2D _rigidbody;
    private GameObject _player;
    private Rigidbody2D _playerRigidbody;
    public bool IsDisabled { get; set; }

    private void Awake()
    {
        _engine = GetComponent<Engine>();
        _health = GetComponent<Health>();
        _weaponController = GetComponentInParent<WeaponController>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _player = GameObject.FindWithTag("Player");
    }

    public void Start()
    {
        _playerRigidbody = _player.GetComponent<Rigidbody2D>();
    }
    
    public void FixedUpdate()
    {
        if (IsDisabled)
            return;
        
        var playerPosition = _playerRigidbody.position;
        var spaceshipPosition = _rigidbody.position;
        var toPlayer = playerPosition - spaceshipPosition;
        var lookVector = transform.rotation * Vector2.up;
        _engine.OnRotate?.Invoke(GetRotationValue(lookVector, toPlayer));
        var distance = toPlayer.magnitude;
        if (distance > 25) 
            return;
        Boost(distance);
        Shoot(distance, toPlayer);
    }

    private void Boost(float distance)
    {
        if (distance > 9)
            _engine.OnBoost?.Invoke(1f);
        else if (distance > 5)
            _engine.OnBoost?.Invoke(0.55f);
        else
            _engine.OnBoost?.Invoke(-0.35f);
    }

    private void Shoot(float distance, Vector2 toTarget)
    {
        if (2 < distance && distance < 8)
            _weaponController.OnShoot?.Invoke(toTarget);
    }
    
    private float GetRotationValue(Vector2 lookVector, Vector2 toObject)
    {
        var mixedMultiplication = lookVector.x * toObject.y - lookVector.y * toObject.x;
        if (Math.Abs(mixedMultiplication) < 1e-9)
            return 0;
        return mixedMultiplication < 0 ? 0.5f : -0.5f;
    }
}