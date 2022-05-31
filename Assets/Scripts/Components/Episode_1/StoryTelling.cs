using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace Components.Episode_1
{
    public class StoryTelling : MonoBehaviour
    {
        private GameObject _utilities;
        private DateTime _inGameTime;
        private bool _endOfFirstDialogue;
        
        private GameObserver _observer;
        private InGameDialog _dialog;
        private PrefabsSpawner _spawner;
        
        private List<Action> _dialoguesQueue;
        
        private void Awake()
        {
            _utilities = GameObject.Find("Utilities");
            _observer = _utilities.GetComponent<GameObserver>();
            _dialog = _utilities.GetComponent<InGameDialog>();
            _spawner = _utilities.GetComponent<PrefabsSpawner>();

            _dialoguesQueue = new List<Action>
            {
                GameStart, FirstEnemy, FirstEnemyKill, NearToStation, FirstStationKill
            };
        }

        private void Update()
        {
            if (_dialoguesQueue.Count == 0) return;

            if (_observer.StationKillsCount > 0 && _dialoguesQueue[0] == FirstStationKill)
            {
                FirstStationKill();
                return;
            }

            if (DistanceToFirstStation() < 30 && _dialoguesQueue[0] == NearToStation)
            {
                NearToStation();
                return;
            }

            if (_observer.EnemyKillsCount > 0 && _dialoguesQueue[0] == FirstEnemyKill)
            {
                FirstEnemyKill();
                return;
            }
            
            if (Math.Sqrt(Math.Pow(_observer.Player.transform.position.x, 2) + Math.Pow(_observer.Player.transform.position.x, 2)) > 3
             && _dialoguesQueue[0] == FirstEnemy)
            {
                SpawnEnemyNearToPlayer(10);
                FirstEnemy();
                return;
            }

            if ((DateTime.Now - _observer.GameStartTime).Seconds > 0 && _dialoguesQueue[0] == GameStart)
                GameStart();
        }

        private void GameStart()
        {
            _dialog.StartDialog(new List<Dialog>
            {
                new Dialog("Твою-ж дивизию,# где я?&" +
                           "Пустота вокруг,# голова раскалывается - отличный день, чтобы умереть.&" +
                           "Прием,# меня кто-нибудь слышит?!# Ау?!", 0, 4),
                new Dialog("А…#к…#о…#я", 3, 2),
                new Dialog("А, что?!# Прием-прием,# меня слышно?!", 3, 4),
                new Dialog("Боже, как я рад кого-то услышать!# Кто вы?# Меня подбили около Земли," +
                           " я потерял сознание, проснулся - и вот уже здесь.## Вокруг пустота," +
                           " ни черта не вижу, связь барахлит. Ало,# ало,# вы слышите меня?", 0, 4),
                new Dialog("*помехи* Да, я тут! Господи, я уже несколько лет не слышала человеческий голос, и не…", 3, 4),
                new Dialog("Что?! О чем это вы? Где вы? Куда я попал?", 0, 4),
                new Dialog("Это долгая история. Давайте сначала разберемся, что нам с вами делать", 3, 4),
                new Dialog("Постой, кажется задержка связи не такая уж и большая." +
                           " Кажется, мы где-то совсем недалеко друг от друга? Где вы?", 0, 4),
                new Dialog("Мой прибор для определения местоположения сломан, придется действовать вслепую," +
                           " вижу из иллюминатора# а*помехи*ыу #*обрыв связи*", 3, 4),
                new Dialog("Екарный бабай, ни черта не понятно. Ало?!# Ало?!", 0, 4),
                new Dialog("Хуем по лбу не дало?# Шкипер, слушай сюда: я встроен в твой корабль и буду тебе, простофиле," +
                           " помогать.# Разберись сначала с управлением, салага", 1, 4),
                new Dialog("Че?", 0, 4),
                new Dialog("Хуй в оче. Видишь на пульте управления кнопки WASD?# На них будешь рулить махиной." +
                           "# Если придется стрелять - жми на ЛКМ или CTRL - они на разных сторонах панели", 1, 4),
                new Dialog("Понял, но# как…ты?# Ладно, понял, лучше лишних слов не говорить", 0, 4),
                new Dialog("Та баба, кажется, где-то рядом, мои алгоритмы проанализировали ваш разговор." +
                           " Попробуй газануть, проверим, сильно ли повредился корабль", 1, 4)
            });
            _dialoguesQueue.RemoveAt(0);
            _endOfFirstDialogue = true;
        }

        private void SpawnEnemyNearToPlayer(int offset)
        {
            var p = _observer.Player.transform.position;
            var enemyPos = new Vector3(p.x + offset, p.y + offset, p.z);
            _spawner.Spawn("Enemy", enemyPos);
        }

        private void FirstEnemy()
        {
            _dialog.StartDialog(new List<Dialog>
            {
                new Dialog("Вроде все довольно неплохо.", 1, 4),
                new Dialog("Стой,# радары показывают, что к нам приближается корабль", 1, 4),
                new Dialog("Немощный старик, мой повелитель отправил меня расправиться с тобой.#" +
                           " Посмотрим на что ты способен", 2, 4),
                new Dialog("Стреляй, жми, екарный бабай", 1, 4)
            });
            _dialoguesQueue.RemoveAt(0);
        }

        private void FirstEnemyKill()
        {
            _dialog.StartDialog(new List<Dialog>
            {
                new Dialog("Дед, ну есть еще порох в пороховницах." +
                           " Раз он здесь появился, значит где-то рядом есть вражеская база." +
                           " Попробуй пролететь подальше, может встретим кого-нибудь", 1, 4)
            }); 
            _dialoguesQueue.RemoveAt(0);
        }

        private double DistanceToFirstStation()
        {
            var p1 = _observer.Player.transform.position;

            return _observer.Stations
                .Select(station => station.transform.position)
                .Select(p2 => Math.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y)))
                .Prepend(double.MaxValue).Min();
        }
        
        private void NearToStation()
        {
            _dialog.StartDialog(new List<Dialog>()
            {
                new Dialog("ты близок к станции врага!", 1, 4)
            }); 
            _dialoguesQueue.RemoveAt(0);
        }
        
        private void FirstStationKill()
        {
            _dialog.StartDialog(new List<Dialog>()
            {
                new Dialog("ты уничтожил свою первую базу!", 1, 4)
            }); 
            _dialoguesQueue.RemoveAt(0);
        }
    }
}