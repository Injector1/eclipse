using UnityEngine;

/*
 Все модули корабля - это некоторые ГО со скриптовым компонентом, лежащие в ГО корабля
 Это контроллер - модуль корабля. Разверни ГО корабля и найди там ГО Controller, один из компонентов которого - этот скрипт
 */

public class PlayerController : MonoBehaviour, ISpaceshipModule 
{
    private Spaceship Spaceship;
    //пока пропустим, можете поизучать класс SavedInput.cs
    private SavedInput<float> horizontalAxis;
    private SavedInput<float> verticalAxis;
    private SavedInput<Vector3> mousePosition;
    
    /*
     Итак, как говорил ранее, при запуске сцены / при создании ГО во всех его скриптовых компонентах вызывается Awake
     Его единственная функция настроить связь между компонетнами/геймобъектами и присовить значения полям класса
     В awake нельзя обращаться к каким либо полям друго класса, так как объект этого класса мог быть еще не проинициализирован
     Для этого создан Start (далее)
     */
    public void Awake()
    {
        /*
         Для каждого ГО модуля корабля его родитель - ГО Spaceship
         Тут с помощью GetComponentInParent получаем в родителе компонент Spaceship, объект (не GameObject) ранее изученного класса
         */
        Spaceship = GetComponentInParent<Spaceship>(); 
        horizontalAxis = new SavedInput<float>();
        verticalAxis = new SavedInput<float>(); 
        mousePosition = new SavedInput<Vector3>();
    }
    
    /*
     После всех Awake-ов скриптов геймобъектов сцены (после инициализации всех объектов, классов), у всех скриптов ГО запускается Start
     Тут уже мы можешь "лезть" в другие классы
     */
    public void Start()
    {
        //добавляем кораблю наш контроллер
        Spaceship.AddModule(transform.gameObject, this.GetType());
    }
    
    /*
     Это Update, у каждого скрипта геймобъектов при наличии эта функция запускается каждый кадр
     */
    public void Update()
    {
        //Проверяем, нажата ли на этом кадре ЛКМ
        if (Input.GetButton("Fire1"))
            //Сохраняем позицию мыши в переменную (далее скажу зачем)
            mousePosition.Value = Input.mousePosition;
        
        //Проверяем, нажал ли пользователь кнопку повота кораблем
        if (Input.GetButton("Horizontal"))
            horizontalAxis.Value = Input.GetAxis("Horizontal");
        
        //Проверяем, нажал ли пользователь кнопку движения кораблем
        if (Input.GetButton("Vertical"))
            verticalAxis.Value = Input.GetAxis("Vertical");
    }
    
    /*
     А это FixedUpdate, запускается каждые 0.02 секунды
     В отличие от Update временной промежуток между вызовами одинаков, что позволяет например достоверно симулировать
     физику, когда в Update скорость физических процессов будет зависить от фпс и мощности компа
    
     Поэтому, чтобы скорость нашего корабля не зависела от фпс, используем FixedUpdate
     А чтобы нажатия например мыши не терялись между FixedUpdate-ами, мы сохраняем нажатия в Update
     */
    public void FixedUpdate()
    {
        if (mousePosition.IsUpdated)
        {
            //вычисляем вектор от корабля до места нажатия мышью
            var lookVector = mousePosition.Value - Camera.main.WorldToScreenPoint(Spaceship.transform.position);
            //при нажатии лкм вызываем OnShoot - набор действий корабля при стрельбе
            //передаем lookVector(пока нигде не используется, на будущее)
            Spaceship.OnShoot?.Invoke(lookVector);
        }

        if (horizontalAxis.IsUpdated) 
            //при нажатии кнопки поворота корабля вызываем набор действий поворота корабля
            Spaceship.OnRotate?.Invoke(horizontalAxis.Value);

        if (verticalAxis.IsUpdated)
        {
            if (verticalAxis.Value > 0)
                Spaceship.OnBoost?.Invoke(verticalAxis.Value);
            else
                Spaceship.OnSlowDown?.Invoke(verticalAxis.Value);
        }
    }
    
    /*
       Продолжение в Spaceship/Modules/BasicMainEngine.cs
     */
}
