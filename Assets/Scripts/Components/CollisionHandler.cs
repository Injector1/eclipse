using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;


public class CollisionHandler : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Health _health;
    [SerializeField] private float PhysDamageMultiplier = 1;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    

    void OnCollisionEnter2D(Collision2D collision)
    {
        var otherObject = collision.gameObject;
        var damageBody = collision.gameObject.GetComponent<DamageBody>();
        if (damageBody is null)
            TakePhysicalDamage(collision);
        else
            HandleCollision(otherObject);
    }

    public void HandleCollision(GameObject otherObject)
    {
        var damageBody = otherObject.GetComponent<DamageBody>();
        _health.OnHealthChange?.Invoke(-damageBody.GetDamage());
    }

    void TakePhysicalDamage(Collision2D collision)
    {
        var rbOther = collision.gameObject.GetComponent<Rigidbody2D>();
        if (_rigidbody is null || rbOther is null)
            return;
        var mass = _rigidbody.bodyType == RigidbodyType2D.Dynamic ? _rigidbody.mass : 1;
        var massOther = rbOther.bodyType == RigidbodyType2D.Dynamic ? rbOther.mass : 0;
        var sqrSpeed = collision.otherCollider.TryGetComponent<OrbitingBody>(out var orbitingBody)
                ? (_rigidbody.velocity - orbitingBody.GetVelocity()).sqrMagnitude
                : collision.relativeVelocity.sqrMagnitude;
        var damage = PhysDamageMultiplier * (sqrSpeed > 0.5f ? sqrSpeed : 0) * massOther / (mass + massOther);
        _health.OnHealthChange?.Invoke(-damage);
    }
}