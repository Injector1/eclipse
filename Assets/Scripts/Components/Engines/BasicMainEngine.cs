using System;
using UnityEngine;


public class BasicMainEngine : Engine
{
    [SerializeField] private float MaxSpeedSqr = 200f;
    [SerializeField] private float Power = 0.1f;
    [SerializeField] private float Maneuverability = 4.5f;
    [SerializeField] private float OnRotateSlowDown = 20f;
    private Rigidbody2D _rigidbody;
    private Transform _spaceshipTransform;
    private Vector3 _spaceshipDirection;
    
    public void Awake()
    {
        _spaceshipDirection = new Vector3(0, 1);
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    public void Start()
    {
        OnRotate += ChangeDirection;
        OnBoost += Boost;
        OnSlowDown += SlowDown;
        _spaceshipTransform = transform;
    }
    
    private void ChangeDirection(float axis)
    {
        _spaceshipTransform.rotation *= Quaternion.AngleAxis(axis * Maneuverability, Vector3.back);
        _spaceshipDirection = transform.rotation * Vector3.up;
    }
    
    private void SlowDown(float axis)
    {
        _rigidbody.AddForce(axis * _rigidbody.velocity * _rigidbody.mass);
    }

    private void Boost(float axis)
    {
        var shVelocity = _rigidbody.velocity;
        var dirMul = 1 - Vector3.Dot(_spaceshipDirection, shVelocity.normalized);
        dirMul = dirMul > 0.1f ? 0.1f : dirMul;
        dirMul *= OnRotateSlowDown;
        SlowDown(-dirMul);
        
        var f = (1 + dirMul) * (1 - shVelocity.sqrMagnitude / MaxSpeedSqr) * Power * axis * _spaceshipDirection;
        _rigidbody.AddForce(f, ForceMode2D.Impulse);
    }
}