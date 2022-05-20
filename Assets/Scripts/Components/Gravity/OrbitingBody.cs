using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class OrbitingBody : MonoBehaviour
{
    [SerializeField] private GameObject OnOrbitOf;
    [SerializeField] private int OrbitDirection;
    private Gravity _gravity;
    [NonSerialized] private Rigidbody2D _rigidbody;
    private float U;
    private float T;
    private float _orbitRadius;
    private float _speed;
    private float _orbitTime;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _gravity = OnOrbitOf.GetComponent<Gravity>();
        GetComponent<GravityVisitor>().IsOnOrbit = true;
        U = (_gravity._rigidbody.mass + _rigidbody.mass) * Gravity.G;
        var dirVector = _gravity.transform.position - transform.position;
        _orbitRadius = dirVector.magnitude;
        T = 2 * Mathf.PI * Mathf.Sqrt(_orbitRadius * _orbitRadius * _orbitRadius / U);
        _speed = Mathf.Sqrt(U / _orbitRadius);
        _orbitTime = T * Vector2.Angle(Vector2.right, -dirVector) / 360;
    }

    public Vector2 GetVelocity()
    {
        var dirVector = _gravity.transform.position - transform.position;
        var normal = (Quaternion.Euler(0, 0, -90 * OrbitDirection) * dirVector).normalized;
        return normal * _speed;
    }

    private void FixedUpdate()
    {
        _orbitTime += Time.fixedDeltaTime;
        if (_orbitTime >= T)
            _orbitTime %= T;
        var t = _orbitTime / T * 2 * Mathf.PI;
        var x = _orbitRadius * Mathf.Cos(OrbitDirection * t);
        var y = _orbitRadius * Mathf.Sin(OrbitDirection * t);
        _rigidbody.velocity = (OnOrbitOf.transform.position + new Vector3(x, y) - transform.position).normalized * _speed;
    }
    
}