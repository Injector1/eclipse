using System;
using UnityEngine;

public class GravityVisitor : MonoBehaviour
{
    public new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void AddGravityForce(Vector3 forceVector)
    {
        rigidbody.AddForce(forceVector);
    }
}