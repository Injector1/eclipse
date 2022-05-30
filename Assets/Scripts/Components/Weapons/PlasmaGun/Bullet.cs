using System.Threading.Tasks;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int LifeTime;
    [SerializeField] private float Damage;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        ActionPlanner.PostponeAnAction(() => Destroy(gameObject), LifeTime);
    }
    
    private async void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Collider2D>().isTrigger)
            return;
        var otherHealth = other.gameObject.GetComponent<Health>();
        if (otherHealth is not null && gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > 1e-9)
            otherHealth.OnHealthChange?.Invoke(-Damage);
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
        _animator.SetBool("isDead", true);
        await Task.Delay(500);
        Destroy(gameObject);
    }
}
