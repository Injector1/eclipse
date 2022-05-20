using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int LifeTime = 5000;

    private void Start()
    {
        EventPlanner.PostponeAnEvent(() => Destroy(gameObject), LifeTime);
    }
}
