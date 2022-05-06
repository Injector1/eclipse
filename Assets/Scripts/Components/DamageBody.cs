using System;
using System.Threading.Tasks;
using UnityEngine;

public class DamageBody : MonoBehaviour
{
    [SerializeField] private float RawDamage = 1;
    [SerializeField] private AudioSource takeDamageSound;

    public float GetDamage()
    {
        takeDamageSound.Play();
        return RawDamage;
    }

    async private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Collider2D>().isTrigger)
            return;
        var otherCollisionHandler = other.gameObject.GetComponent<CollisionHandler>();
        if (!(otherCollisionHandler is null))
            otherCollisionHandler.HandleCollision(this.gameObject);
        if (!(other.gameObject.GetComponent<Collider2D>() is null))
        {
            await Task.Delay(300);
            Destroy(gameObject);
        }
    }
}