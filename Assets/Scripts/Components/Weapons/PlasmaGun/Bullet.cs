using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int LifeTime;
    [SerializeField] private float Damage;

    private void Start()
    {
        this.StartCoroutine(() => Destroy(gameObject), LifeTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Collider2D>().isTrigger)
            return;
        var otherHealth = other.gameObject.GetComponent<Health>();
        if (!(otherHealth is null))
            otherHealth.OnHealthChange?.Invoke(-Damage);
        Destroy(gameObject);
    }
}
