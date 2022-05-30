using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class InterestingGameManager : MonoBehaviour
{
    private GameObject _player;
    private Health _playerHealth;
    private GameObserver _gameObserver;
    private bool _isGameEnded;
    [SerializeField] public GameObject loseFrame;
    [SerializeField] private GameObject winFrame;
    [SerializeField] private int secondsToGetMaxRating;
    [SerializeField] private int secondsToGetMediumRating;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _playerHealth = _player.GetComponent<Health>();
        _gameObserver = GameObject.FindWithTag("Utilities").GetComponent<GameObserver>();
    }

    private void Start()
    {
        _playerHealth.OnDeath -= _playerHealth.BasicDeath;
        _playerHealth.OnDeath += PlayerDeath;
        _gameObserver.OnEntityDeath += _ => CheckStationCount();
    }

    private void PlayerDeath()
    {
        Time.timeScale = 0.2f;
        loseFrame.SetActive(true);
        //EventPlanner.PostponeAnEvent(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name), 1200);
    }
    
    private void CheckStationCount()
    {
        GameObject.FindWithTag("Utilities").GetComponent<InGameDialog>().StartDialog("как же я ненавижу чурок", 0, 1);
        var n = _gameObserver.Stations.Count;

        if (n == 0 && !_isGameEnded) winFrame.GetComponent<CheckForWin>().GetEpisodeResult(
                GetRating(),
                (DateTime.Now - _gameObserver.GameStartTime).Seconds);
    }

    private int GetRating()
    {
        _isGameEnded = true;

        var gameTime = (DateTime.Now - _gameObserver.GameStartTime).Seconds;
        
        if (gameTime < secondsToGetMaxRating)
            return 3;
        return gameTime < secondsToGetMediumRating 
                ? 2 
                : 1;
    }
}