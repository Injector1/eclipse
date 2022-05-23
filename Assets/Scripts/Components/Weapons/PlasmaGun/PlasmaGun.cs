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
        _weaponController.OnShoot += Shoot;
        _bullet = transform.GetChild(0).gameObject;
    }

    private void Shoot(Vector3 target)
    {
        var bulletCount = GameObject.FindWithTag("Gun").name == "Semi-automatic"
        ? 5
        : 1;
        
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