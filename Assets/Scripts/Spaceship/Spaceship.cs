using UnityEngine;
using System;
using System.Collections.Generic;

/*
 Hey hey hey
 Если это первое что ты читаешь, ты уже на верном пути
 Итак, ща будет быстрый (или не очень) экскурс по написанию и поддержки ультрачитаемого и ультрамасштабируемого кода
 
 Начнем с базы
 Условно говоря в юнити на сцене два типа объектов - GameObject-ы и их компоненты
 !Важно: все скрипты и все классы в этих скриптах это либо компоненты GameObject либо просто скрипты :)
         все классы, взаимодействующие с гей мобъектами должны наследоваться от MonoBehaviour (так надо)
 
 Рассмотрим базовый класс - Spaceship
 Spaceship это не сам корабль, это компонент геймобъекта (далее ГО) Spaceship. Найди его на сцене SampleScene в 
 Hierarchy (Main Scene -> Spaceship). По сути этот класс это маршрутизатор, который позволяет другим объектам взаиомдействовать с 
 ГО Spaceship. Как видно, сам по себе он практически ничего не предстваляет, он явлется фундаментом, над которым
 другие классы строят уже полноценный корабль. Это и есть ключевая идея паттерна наблюдатель и декоратор. Далее подробнее.
 
 p.s прочитанный текст советую удалять для читаемости кода
 */

public class Spaceship : MonoBehaviour
{
    //компонент ГО Spaceship, добавляет к кораблю физическое тело
    public Rigidbody2D Rigidbody;
    
    /*
     А вот и задатки паттерна Observer
     Мы определяем лишь возможные действия корабля, а их реализацией занимаются другие классы
     */
    public Action<Vector3> OnShoot;
    public Action<float> OnRotate;
    public Action<float> OnBoost;
    public Action<float> OnSlowDown;
    
    /*
     при запуске сцены / при создании ГО во всех скриптовых компонентах вызывается Awake
     подробнее расскажу в PlayerController.cs
     */
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Modules = new Dictionary<Type, HashSet<GameObject>>();
    }
    
    /*
     Модули корабля (движок, пушки, контроллеры, аниматоры и прочее) и фунция добавления к кораблю модуля
     (паттерн декоратор + observer)
     */
    public Dictionary<Type, HashSet<GameObject>> Modules;
    
    public void AddModule(GameObject module, Type moduleType)
    {
        if (!Modules.TryGetValue(moduleType, out var modulesSet))
            modulesSet = new HashSet<GameObject>();
        modulesSet.Add(module);
    }
    
    //Вот и весь Spaceship, щас перейди в PlayerController.cs (папка Spaceship -> Modules -> Controllers)
}
