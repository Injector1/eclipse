/*using System;
using System.Linq;
using UnityEngine;

public class PhysicalBody : MonoBehaviour
{
    [SerializeField] private float MassSF;
    [SerializeField] private Vector2 VelocitySF;
    
    public float Mass { get; set; }

    private Vector2 velocity;
    public Vector2 Velocity
    {
        get => GetVelocity.Invoke();
        set => velocity = GetVelocity is null ? value : Vector2.zero;
    }
    public Func<Vector2> GetVelocity;

    private Action _onUpdate;
    
    public void upda
    
    public void AddForce(Vector2 force)
    {
        Velocity += force / Mass;
    }

    public void AddVelocity(Vector2 velocity)
    {
        Velocity += velocity;
    }

    private void OnValidate()
    {
        Mass = MassSF;
        Velocity = VelocitySF;
    }

    private void Move()
    {
        
    }

    private void Update()
    {
        _onUpdate?.Invoke();
    }
}*/