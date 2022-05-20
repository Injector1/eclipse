using System;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public const float G = 100f;

    public Rigidbody2D _rigidbody;
    private HashSet<GravityVisitor> visitors;
    private HashSet<GravityVisitor> orbitingBodies;

    private void Awake()
    {
        orbitingBodies = new HashSet<GravityVisitor>();
        visitors = new HashSet<GravityVisitor>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        foreach (var visitor in visitors)
            try
            {
                var direction = transform.position - visitor.transform.position;
                var gravityForce = G * _rigidbody.mass * visitor._rigidbody.mass / direction.sqrMagnitude;
                visitor.AddGravityForce(direction.normalized * gravityForce);
            }
            catch (Exception e)
            {
                visitors.Remove(visitor);
                break;
            }
        
    }

    public void OnEnterGravityZoneTrigger(Collider2D other)
    {
        var visitor = other.GetComponent<GravityVisitor>();
        if (visitor != null && !visitor.IsOnOrbit && !orbitingBodies.Contains(visitor))
            visitors.Add(visitor);
    }
    
    public void OnExitGravityZoneTrigger(Collider2D other)
    {
        var visitor = other.GetComponent<GravityVisitor>();
        visitors.Remove(visitor);
    }
}