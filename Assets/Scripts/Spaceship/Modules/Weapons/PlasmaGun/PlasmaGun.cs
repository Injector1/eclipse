using UnityEngine;
using System;
using System.Threading.Tasks;
using Random = System.Random;

/*
 Плазменная пушка, модуль корабля
 */
public class PlasmaGun : MonoBehaviour, IWeapon, ISpaceshipModule
{
    [SerializeField] private float Spreading = 10f;
    [SerializeField] private int CoolDownDuration;
    private Spaceship Spaceship;
    private GameObject bullet;
    private CoolDown coolDown;
    private Random random;

    public void Awake()
    {
        Spaceship = GetComponentInParent<Spaceship>();
        coolDown = new CoolDown(CoolDownDuration);
        random = new Random();
    }
    
    public void Start()
    {
        Spaceship.OnShoot += Shoot;
        Spaceship.AddModule(gameObject, typeof(IWeapon));
        //пуля - неактивный ГО, находящийся в ГО плазменной пушки (ребенок геймобъекта Gun)
        coolDown = new CoolDown(CoolDownDuration);
        random = new Random();
        bullet = transform.GetChild(0).gameObject;
    }
    
    private void Shoot(Vector3 target)
    {
        //костыль для изменения кд во время игры
        if (CoolDownDuration != coolDown.CoolDownInMs)
            coolDown.CoolDownInMs = CoolDownDuration;
        
        //если кулдаун - не стреляем (самостоятельно изучить класс CoolDown)
        if (!coolDown.TryToShoot())
            return;
        
        var spreadingAngle = Quaternion.Euler(0, 0, Spreading * (float) (0.5 - random.NextDouble()));
        
        //Instantiate - создает клона с позицией transform.position (позиция ГО PlasmaGun)
        // и с поворотом transform.rotation * spreadingAngle (поворот пушки + разброс)
        var newBullet = Instantiate(bullet, transform.position, transform.rotation * spreadingAngle);
        
        //задаем родителя - ГО Bullets в ГО Shared
        newBullet.transform.parent = GameObject.FindWithTag("Bullets").transform;
        //изначальная пуля неактивна, поэтому делаем клон активным
        newBullet.SetActive(true);
    }
    
    /*
     На этом всё, думаю этого достаточно для понимания работы нашего кода и того, как добавлять новый функционал
     */
}