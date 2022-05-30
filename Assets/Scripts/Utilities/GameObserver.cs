using UnityEngine;

public class GameObserver : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject winFrame;
    [SerializeField] private int secondsToGetMaxRating;
    [SerializeField] private int secondsToGetMediumRating;

    private float _gameStartTime;
    private bool _isGameEnded;

    private Vector3 _playerPosition;

    private int _enemyCount;
    private int _killsCount;

    private int _stationCount;

    private void Start()
    {
        _playerPosition = new Vector3();
        _enemyCount = 0;
        _killsCount = 0;
        _stationCount = GameObject.FindGameObjectsWithTag("Station").Length;;
        _gameStartTime = Time.time;
        _isGameEnded = false;
    }

    void Update()
    {
        UpdatePlayerPosition();
        UpdateEnemyCount();
        UpdateStationCount();
    }

    private void UpdatePlayerPosition()
    {
        _playerPosition = player.transform.position;
    }

    private void UpdateEnemyCount()
    {
        var n = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (n > _enemyCount) _enemyCount = n;
        else if (n < _enemyCount) { _enemyCount = n; _killsCount++; }
    }

    private void UpdateStationCount()
    {
        var n = GameObject.FindGameObjectsWithTag("Station").Length;
        if (n < _stationCount) { _stationCount = n; }

        if (_stationCount == 0 && !_isGameEnded) winFrame.GetComponent<CheckForWin>().GetEpisodeResult(
            GetRating(),
            Time.time - _gameStartTime);
    }

    private int GetRating()
    {
        _isGameEnded = true;

        var gameTime = Time.time - _gameStartTime;
        
        if (gameTime < secondsToGetMaxRating)
            return 3;
        return gameTime < secondsToGetMediumRating 
            ? 2 
            : 1;
    }

    public Vector3 GetPlayerPosition()
    {
        return _playerPosition;
    }

    public int GetKillsCount()
    {
        return _killsCount;
    }
}
