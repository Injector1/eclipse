using System;
using UnityEngine;

public class GravityVisitor : MonoBehaviour
{
    [NonSerialized] public Rigidbody2D _rigidbody;
    public bool IsOnOrbit;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void AddGravityForce(Vector3 forceVector)
    {
        _rigidbody.AddForce(forceVector);
    }
}