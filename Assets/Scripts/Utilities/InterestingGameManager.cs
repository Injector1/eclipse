using UnityEngine;
using UnityEngine.SceneManagement;

public class InterestingGameManager : MonoBehaviour
{
    private GameObject _player;
    private Health _playerHealth;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _playerHealth = _player.GetComponent<Health>();
    }

    private void Start()
    {
        _playerHealth.OnDeath -= _playerHealth.BasicDeath;
        _playerHealth.OnDeath += PlayerDeath;
    }

    private void PlayerDeath()
    {
        EventPlanner.PostponeAnEvent(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name), 1200);
    }
}