using UnityEngine;
using System;
using System.Threading.Tasks;
using Random = System.Random;

public class PlasmaGun : Weapon
{
    [SerializeField] private float Spreading;
    [SerializeField] private GameObject GameObjectWithBullets;
    [SerializeField] private AudioSource shootSound;

    private WeaponController _weaponController;
    private CoolDown _coolDown;
    private GameObject _bullet;
    private Random _random;

    
    public void Awake()
    {
        _coolDown = GetComponent<CoolDown>();
        _weaponController = GetComponentInParent<WeaponController>();
        GameObjectWithBullets = GameObject.FindWithTag("Bullets");
        _random = new Random();
    }
    
    public void Start()
    {
        _weaponController.OnShoot += Shoot;
        _bullet = transform.GetChild(0).gameObject;
    }
    
    private void Shoot(Vector3 target)
    {
        if (!shootSound.isPlaying)
            shootSound.Play();
        if (!_coolDown.TryToShoot())
            return;
        
        var spreadingAngle = Quaternion.Euler(0, 0, Spreading * (float) (0.5 - _random.NextDouble()));
        var newBullet = Instantiate(_bullet, transform.position, transform.rotation * spreadingAngle);
        
        newBullet.transform.parent = GameObjectWithBullets.transform;
        newBullet.SetActive(true);
    }
}