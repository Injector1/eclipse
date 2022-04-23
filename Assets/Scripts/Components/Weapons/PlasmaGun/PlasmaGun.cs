using UnityEngine;
using System;
using System.Threading.Tasks;
using Random = System.Random;

public class PlasmaGun : MonoBehaviour, IWeapon
{
    [SerializeField] private float Spreading;
    [SerializeField] private GameObject GameObjectWithBullets;

    private Actions _actions;
    private CoolDown _coolDown;
    private GameObject _bullet;
    private Random _random;

    
    public void Awake()
    {
        _actions = GetComponentInParent<Actions>();
        _coolDown = GetComponent<CoolDown>();
        GameObjectWithBullets = GameObject.FindWithTag("Bullets");
        _random = new Random();
    }
    
    public void Start()
    {
        _actions.OnShoot += Shoot;
        _bullet = transform.GetChild(0).gameObject;
    }
    
    private void Shoot(Vector3 target)
    {
        if (!_coolDown.TryToShoot())
            return;
        
        var spreadingAngle = Quaternion.Euler(0, 0, Spreading * (float) (0.5 - _random.NextDouble()));
        var newBullet = Instantiate(_bullet, transform.position, transform.rotation * spreadingAngle);
        
        newBullet.transform.parent = GameObjectWithBullets.transform;
        newBullet.SetActive(true);
    }
}