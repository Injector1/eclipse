using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Utilities;

namespace Components.Episode_1
{
    public class StoryTelling : MonoBehaviour
    {
        [SerializeField] private GameObject planet;
        [SerializeField] private GameObject[] spawners;
        
        private GameObject _utilities;
        private DateTime _clearPlanetTime;

        private CheckForWin _winChecker;
        private GameObserver _observer;
        private InGameDialog _dialog;
        private PrefabsSpawner _spawner;
        
        private List<Action> _dialoguesQueue;
        
        private void Awake()
        {
            _utilities = GameObject.Find("Utilities");
            _winChecker = _utilities.GetComponent<CheckForWin>();
            _observer = _utilities.GetComponent<GameObserver>();
            _dialog = _utilities.GetComponent<InGameDialog>();
            _spawner = _utilities.GetComponent<PrefabsSpawner>();

            _dialoguesQueue = new List<Action>
            {
                GameStart, FirstEnemy, FirstEnemyKill, NearToStation, FirstStationKill, NearToPlanet, ClearPlanet,
                 FarFromPlanet
            };
        }

        private void Update()
        {
            if (_dialoguesQueue.Count == 0) return;
            
            if ((DateTime.Now - _clearPlanetTime).Seconds > 10 && _dialoguesQueue[0] == FarFromPlanet)
            {
                FarFromPlanet();
                return;
            }
            
            if (spawners.All(s => !s.activeSelf) && _dialoguesQueue[0] == ClearPlanet)
            {
                _clearPlanetTime = DateTime.Now;
                ClearPlanet();
                return;
            }
            
            if (DistanceToPlanet() < 40 && _dialoguesQueue[0] == NearToPlanet)
            {
                NearToPlanet();
                return;
            }

            if ((DateTime.Now - _observer.FirstStationKillTime).Seconds > 1  &&
                _observer.StationKillsCount > 0 && _dialoguesQueue[0] == FirstStationKill)
            {
                FirstStationKill();
                return;
            }

            if (DistanceToFirstStation() < 40 && _dialoguesQueue[0] == NearToStation)
            {
                NearToStation();
                return;
            }

            if ((DateTime.Now - _observer.FirstKillTime).Seconds > 0.8 
                && _observer.EnemyKillsCount > 0 && _dialoguesQueue[0] == FirstEnemyKill)
            {
                FirstEnemyKill();
                return;
            }
            
            if (Math.Sqrt(Math.Pow(_observer.Player.transform.position.x, 2) + Math.Pow(_observer.Player.transform.position.y, 2)) > 10
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
                new Dialog("Твою-ж дивизию, где я?&" +
                           "Пустота вокруг, голова раскалывается - отличный день, чтобы умереть.&" +
                           "Прием,# меня кто-нибудь слышит?! Ау?!", 0, 4),
                new Dialog("$А…к…о…я$", 3, 2),
                new Dialog("А, что?! Прием-прием, меня слышно?!", 3, 4),
                new Dialog("Боже, как я рад кого-то услышать! Кто вы? Меня подбили около Земли," +
                           " я потерял сознание, проснулся - и вот уже здесь. Вокруг пустота," +
                           " ни черта не вижу, связь барахлит. Ало,# ало,# вы слышите меня?", 0, 4),
                new Dialog("$#$Да, я тут! Господи, я уже несколько лет не слышала человеческий голос, и не…$#$", 3, 4),
                new Dialog("Что?! О чем это вы? Где вы? Куда я попал?", 0, 4),
                new Dialog("Это долгая история.  Я тоже попала в подобную ситуацию", 3, 4),
                new Dialog("Что? Как? Не с Земли? Постой, кажется задержка связи не такая уж и большая." +
                           " Кажется, мы где-то совсем недалеко друг от друга? Где вы?", 0, 4),
                new Dialog("Мой прибор для определения местоположения сломан, придется действовать вслепую," +
                           " вижу из иллюминатора $#а…#ы…#у…#$ *обрыв связи*", 3, 4),
                new Dialog("Екарный бабай, ни черта не понятно. Ало?! Ало?!", 0, 4),
                new Dialog("Хреном по лбу не дало?# Шкипер, слушай сюда: я встроен в твой корабль и буду тебе, простофиле," +
                           " помогать.# Разберись сначала с управлением, салага", 1, 4),
                new Dialog("Че?", 0, 4),
                new Dialog("Хрен через плечо. Видишь на пульте управления кнопки WASD? На них будешь рулить махиной." +
                           " Если придется стрелять - жми на ЛКМ или CTRL - они на разных сторонах панели.&" +
                           "Также можешь отдалять зону видимости на скролл колесика на приборной панели.", 1, 4),
                new Dialog("Понял, но как…ты? Ладно, понял, лучше лишних слов не говорить", 0, 4),
                new Dialog("Та баба, кажется, где-то рядом, мои алгоритмы проанализировали ваш разговор." +
                           " Попробуй газануть, проверим, сильно ли повредился корабль", 1, 4)
            });
            _dialoguesQueue.RemoveAt(0);
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
                new Dialog("Вроде все довольно неплохо.&" +
                           "Стой, радары показывают, что к нам приближается корабль. " +
                           "Красная стрелочка на дисплее указывает на него", 1, 4),
                new Dialog("Немощный старик, мой повелитель отправил меня расправиться с тобой.#" +
                           " Надеюсь, ты уже выбрал себе могилу", 4, 4),
                new Dialog("Стреляй, екарный бабай", 1, 4)
            });
            _dialoguesQueue.RemoveAt(0);
        }

        private void FirstEnemyKill()
        {
            _dialog.StartDialog(new List<Dialog>
            {
                new Dialog("Дед, ну есть еще порох в пороховницах." +
                           " Раз он здесь появился, значит где-то рядом есть вражеская база." +
                           " Попробуй пролететь подальше, может встретим кого-нибудь.&" +
                           "Следуй за желтой стрелочкой на радаре. Она приведет к вражеской базе", 1, 4)
            }); 
            _dialoguesQueue.RemoveAt(0);
        }

        private double DistanceToPlanet()
        {
            var p1 = _observer.Player.transform.position;
            var p2 = planet.transform.position;

            return Math.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
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
                new Dialog("Екарный бабай, что это такое? Почему ты меня не предупредил?", 0, 4),
                new Dialog("Я че по-твоему, провидец что ли? Сбавь газу и атакуй их!", 1, 4)
            }); 
            _dialoguesQueue.RemoveAt(0);
        }
        
        private void FirstStationKill()
        {
            _dialog.StartDialog(new List<Dialog>()
            {
                new Dialog("Отлично сработано, салага.", 1, 4),
                new Dialog("$…#$&!Ало!? Ало!? Меня слышно?$#$", 3, 4),
                new Dialog("Это снова вы?! Да, вас слышно, связь стала лучше.", 0, 4),
                new Dialog("Я видела огромный взрыв из иллюминатора, это вы?", 3, 4),
                new Dialog("Что это было?! Какого хрена? Что это еще за империя? Что за повелитель", 0, 4),
                new Dialog("Империя… Они захватывают галактику за галактикой, кажется, уже подобрались к нашей земле." +
                           " Кажется, ты уничтожил одну из их баз.&" +
                           "У них сотни баз по всей галактике. Они все служат своему господину - Нику Роббену." +
                           " Они называют его “Повелитель”", 3, 4),
                new Dialog("Господи, святые угодники!", 0, 4),
                new Dialog("Спокойно, кажется мы с вами уже совсем рядом. Я нахожусь около огромной алой планеты," +
                           " поспешите, вы их разозлил…$#*обрыв связи*#$", 3, 4),
                new Dialog("Черт, кажется, на нас открыли открыли охоту", 0, 4),
                new Dialog("На кого “на нас”, старый пердун? Я - машина.&" +
                           "Кстати, шкипер, пока не забыл. Выключай деменцию и запоминай, что я тебе говорю." +
                           " На дисплее есть две полоски - красная и бирюзовая. Они указывают на состояние твоего корабля&" +
                           "Красная - жизненно необходимый щит для корабля. Бирюзовая - улучшенная защита." +
                           " Она восполняется со временем", 1, 4),
                new Dialog("Ух, веселый денек нас ждет", 0, 4),
                new Dialog("Ты слышал, что сказала девушка? Мы должны искать огромную красную планету. " +
                           "Приборы показывают, что в нашем диапазоне видимости есть только одна система планет.&" +
                           "Я ее тебе пометил на дисплее - следуй за указателем.", 1, 4)
            }); 
            _dialoguesQueue.RemoveAt(0);
        }
        
        private void NearToPlanet()
        {
            _dialog.StartDialog(new List<Dialog>
            {
                new Dialog("Прием!? Прием!? Мы у красной планеты", 0, 4),
                new Dialog("*зловещий смех*", 3, 4),
                new Dialog("Что, старик? Прилетел навестить свою новую подружку?&" +
                           "Мои ребята перехватили ваши разговоры, теперь она у меня. Солдаты, уничтожить его!", 2, 4),
                new Dialog("Черт, засада", 0, 4),
                new Dialog("Бренчи костями, хрыщ, иначе бабка будет оплакивать твой пепел.&" +
                           "Стреляй!", 1, 4)
            }); 
            _dialoguesQueue.RemoveAt(0);
        }
        
        private void ClearPlanet()
        {
            _dialog.StartDialog(new List<Dialog>
            {
                new Dialog("Фух, кажется отбились", 1, 4),
                new Dialog("Ты победил в битве, но в войне! Жди, скоро настанет твой час.", 2, 4)
            }); 
            _dialoguesQueue.RemoveAt(0);
        }
        
        private void FarFromPlanet()
        {
            var rating = 1;
            var gameTime = DateTime.Now - _observer.GameStartTime;
            if (gameTime.Seconds < 3 * 60) rating = 3;
            else if (gameTime.Seconds < 5 * 60) rating = 2;
            _winChecker.GetEpisodeResult(rating, gameTime.Minutes, gameTime.Seconds);
            _dialoguesQueue.RemoveAt(0);
        }
    }
}