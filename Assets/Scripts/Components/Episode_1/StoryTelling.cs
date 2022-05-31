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

            if ((DateTime.Now - _observer.GameStartTime).Seconds > 7 && _dialoguesQueue[0] == FirstEnemy)
            {
                SpawnEnemyNearToPlayer(10);
                FirstEnemy();
                return;
            }

            if ((DateTime.Now - _observer.GameStartTime).Seconds > 0.1 && _dialoguesQueue[0] == GameStart)
                GameStart();
        }

        private void GameStart()
        {
            _dialog.StartDialog(new List<Dialog>
            {
                new Dialog("это начало игры!", 1, 4)
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
                new Dialog("радары засекли поблизости врага, будь осторожен!&Красный указатель поможет тебе понять," +
                           " где находится враг", 1, 4)
            });
            _dialoguesQueue.RemoveAt(0);
        }

        private void FirstEnemyKill()
        {
            _dialog.StartDialog(new List<Dialog>
            {
                new Dialog("это твой первый килл!", 1, 4)
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