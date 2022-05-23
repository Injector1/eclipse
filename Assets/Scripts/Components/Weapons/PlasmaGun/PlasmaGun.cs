using UnityEngine;
using System;
using System.Threading.Tasks;
using Random = System.Random;

public class PlasmaGun : Weapon
{
    [SerializeField] private float Spreading;
    [SerializeField] private GameObject SpawnTo;

    private WeaponController _weaponController;
    private CoolDown _coolDown;
    private GameObject _bullet;
    private Random _random;

    
    public void Awake()
    {
        _coolDown = GetComponent<CoolDown>();
        _weaponController = GetComponentInParent<WeaponController>();
        SpawnTo = GameObject.FindWithTag("Bullets");
        _random = new Random();
    }
    
    public void Start()
    {
        _weaponController.OnShoot += GameObject.FindWithTag("Gun").name switch
        {
            "Semi-automatic" => QueueShoot,
            _ => Shoot
        };

        _bullet = transform.GetChild(0).gameObject;
    }

    private void Shoot(Vector3 target)
    {
        if (!_coolDown.TryToShoot())
            return;
        
        var spreadingAngle = Quaternion.Euler(0, 0, Spreading * (float) (0.5 - _random.NextDouble()));
        var newBullet = Instantiate(_bullet, transform.position, transform.rotation * spreadingAngle);
        
        newBullet.transform.parent = SpawnTo.transform;
        newBullet.SetActive(true);
    }
    
    private void QueueShoot(Vector3 target)
    {
        const int bulletCount = 5;
        
        if (!_coolDown.TryToShoot())
            return;
        

        for (int i = 0; i < bulletCount; i++)
        {
            var spreadingAngle = Quaternion.Euler(0, 0, Spreading * (float) (0.5 - _random.NextDouble()));
            var newBullet = Instantiate(_bullet, transform.position, transform.rotation * spreadingAngle);

            newBullet.transform.parent = SpawnTo.transform;
            newBullet.SetActive(true);
        }
    }
}