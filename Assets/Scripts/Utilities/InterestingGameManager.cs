﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class InterestingGameManager : MonoBehaviour
{
    private GameObject _player;
    private Health _playerHealth;
    [SerializeField] public GameObject loseFrame;

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
        Time.timeScale = 0.2f;
        loseFrame.SetActive(true);
        //EventPlanner.PostponeAnEvent(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name), 1200);
    }
}