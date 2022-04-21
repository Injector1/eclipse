using System;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public const float G = 0.001f;

    private new Rigidbody2D rigidbody;
    private CircleCollider2D planetCollider;
    private GravityZoneTrigger _gravityZoneTrigger;
    private HashSet<GravityVisitor> visitors;

    private void Awake()
    {
        planetCollider = GetComponent<CircleCollider2D>();
        _gravityZoneTrigger = GetComponentInChildren<GravityZoneTrigger>();
        visitors = new HashSet<GravityVisitor>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        foreach (var visitor in visitors)
        {
            var direction = transform.position - visitor.transform.position;
            var gravityForce = G * rigidbody.mass * visitor.rigidbody.mass / direction.sqrMagnitude;
            visitor.AddGravityForce(direction.normalized * gravityForce);
        }
    }

    public void OnEnterGravityZoneTrigger(Collider2D other)
    {
        var visitor = other.GetComponent<GravityVisitor>();
        if (visitor != null)
            visitors.Add(visitor);
    }
    
    public void OnExitGravityZoneTrigger(Collider2D other)
    {
        var visitor = other.GetComponent<GravityVisitor>();
        visitors.Remove(visitor);
    }
}