using Components;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int LifeTime;
    [SerializeField] private float Damage;

    private ParticlesSystem _particles;
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
        PlayExplosion(1);
        Destroy(gameObject);
    }

    private void PlayExplosion(int power)
    {
        var position = gameObject.transform.position;
        GameObject.Find("Utilities").GetComponent<ParticlesSystem>().PlayNewParticles(1, new Vector2(position.x, position.y));
    } 
}
