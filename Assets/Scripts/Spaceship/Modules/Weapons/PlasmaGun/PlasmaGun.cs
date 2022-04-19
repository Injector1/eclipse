using UnityEngine;
using System;
using System.Threading.Tasks;
using Random = System.Random;

public class PlasmaGun : MonoBehaviour, IWeapon, ISpaceshipModule
{
    [SerializeField] private float Spreading;
    [SerializeField] private GameObject GameObjectWithBullets;
    private CoolDown coolDown;
    private Spaceship spaceship;
    private GameObject bullet;
    private Random random;

    public void Awake()
    {
        spaceship = GetComponentInParent<Spaceship>();
        coolDown = GetComponent<CoolDown>();
        GameObjectWithBullets = GameObject.FindWithTag("Bullets");
        random = new Random();
    }
    
    public void Start()
    {
        spaceship.OnShoot += Shoot;
        spaceship.AddModule(this, typeof(IWeapon));
        bullet = transform.GetChild(0).gameObject;
    }
    
    private void Shoot(Vector3 target)
    {
        if (!coolDown.TryToShoot())
            return;
        
        var spreadingAngle = Quaternion.Euler(0, 0, Spreading * (float) (0.5 - random.NextDouble()));
        var newBullet = Instantiate(bullet, transform.position, transform.rotation * spreadingAngle);
        
        newBullet.transform.parent = GameObjectWithBullets.transform;
        newBullet.SetActive(true);
    }
}