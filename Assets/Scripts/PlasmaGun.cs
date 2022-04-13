using UnityEngine;
using System;
using System.Threading.Tasks;

public class PlasmaGun : MonoBehaviour
{
    [SerializeField] private Spaceship spaceship;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject shared;
    

    private void Awake()
    {
        spaceship = GetComponentInParent<Spaceship>();
        spaceship.OnShoot += Shoot;
    }

    private void Shoot(Vector3 target)
    {
        var transform1 = transform;
        var newBullet = Instantiate(bullet, transform1.position, transform1.rotation);
        newBullet.transform.parent = bullet.transform.parent;
        newBullet.SetActive(true);
    }
}