using UnityEngine;

public class Bullet : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    [SerializeField] private int LifeTime = 5;
    [SerializeField] private float Speed = 2;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidbody.velocity =  Speed * (transform.rotation * Vector2.up);
        var collider = GetComponent<Collider2D>();
        EventPlanner.PostponeAnEvent(() => collider.enabled = true, LifeTime);
        EventPlanner.PostponeAnEvent(() => Destroy(gameObject), 3000);
    }
}
