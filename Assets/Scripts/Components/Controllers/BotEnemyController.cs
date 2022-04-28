using System;
using UnityEditor;
using UnityEngine;


public class BotEnemyController : MonoBehaviour
{
    private Engine _engine;
    private Health _health;
    private WeaponController _weaponController;
    private Rigidbody2D _rigidbody;
    private GameObject _player;
    private Rigidbody2D _playerRigidbody;

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
        var playerPosition = _playerRigidbody.position;
        var spaceshipPosition = _rigidbody.position;
        var toPlayer = playerPosition - spaceshipPosition;
        var lookVector = transform.rotation * Vector2.up;
        _engine.OnRotate?.Invoke(GetRotationValue(lookVector, toPlayer));
        if (toPlayer.magnitude < 5)
        {
            _engine.OnBoost?.Invoke(0.5f);
            _weaponController.OnShoot?.Invoke(toPlayer);
        }
        else
        {
            _engine.OnBoost?.Invoke(1);
        }
    }
    
    private float GetRotationValue(Vector2 lookVector, Vector2 toObject)
    {
        var mixedMultiplication = lookVector.x * toObject.y - lookVector.y * toObject.x;
        if (Math.Abs(mixedMultiplication) < 1e-9)
            return 0;
        return mixedMultiplication < 0 ? 0.5f : -0.5f;
    }
}