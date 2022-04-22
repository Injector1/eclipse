using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class CollisionHandler : MonoBehaviour
{
    private Health health;
    private Rigidbody2D rb;
    [SerializeField] private float DamageResist = 1;
    [SerializeField] private float PhysDamageMultiplier = 1;

    private void Awake()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
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
        health.OnHealthChange(-damageBody.GetDamage());
    }

    void TakePhysicalDamage(Collision2D collision)
    {
        var rbOther = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb is null || rbOther is null)
            return;
        var mass = rb.bodyType == RigidbodyType2D.Dynamic ? rb.mass : 1;
        var massOther = rbOther.bodyType == RigidbodyType2D.Dynamic ? rbOther.mass : 0;
        var sqrSpeed = collision.relativeVelocity.sqrMagnitude;
        var damage = PhysDamageMultiplier * (sqrSpeed > 0.5f ? sqrSpeed : 0) * massOther / (mass + massOther);
        health.OnHealthChange?.Invoke(-damage);
    }
}