using System;
using UnityEngine;

public class DamageBody : MonoBehaviour
{
    [SerializeField] private float RawDamage = 1;

    public float GetDamage()
    {
        return RawDamage;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Collider2D>().isTrigger)
            return;
        var otherCollisionHandler = other.gameObject.GetComponent<CollisionHandler>();
        if (!(otherCollisionHandler is null))
            otherCollisionHandler.HandleCollision(this.gameObject);
        if (!(other.gameObject.GetComponent<Collider2D>() is null))
            Destroy(gameObject);
    }
}