using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckForWin : MonoBehaviour
{
    [SerializeField] public Text alienScore;
    [SerializeField] public Text stationScore;
    [SerializeField] public Text result;
    [SerializeField] public GameObject victoryWindow;
    [SerializeField] public GameObject loseWindow;
    [SerializeField] public GameObject[] ratings;
    [SerializeField] public GameObject[] enemies;

    private float startTime;
    private float gameTime;

    private void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (enemies.Any(enemy => enemy.activeInHierarchy))
        {
            return;
        }

        if (!loseWindow.activeSelf)
            GetEpisodeResult();
    }

    private void GetEpisodeResult()
    {
        if (gameTime < 1e-9)
        {
            Time.timeScale = 0f;

            int rating = 0;

            gameTime = Time.time - startTime;
            result.text = gameTime.ToString();
            victoryWindow.SetActive(true);

            if (gameTime > 20) rating = 1;
            else if (gameTime > 10) rating = 2;
            else if (gameTime > 0) rating = 3;

            ratings[rating].SetActive(true);
            
            PlayerPrefs.SetInt("Rating", Max(rating, PlayerPrefs.GetInt("Rating")));
        }
    }

    private int Max(int a, int b)
    {
        return a > b ? a : b;
    }

    public void LoadMenu()
    {
        victoryWindow.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    
    public void RetryLevel()
    {
        Time.timeScale = 1f;
        loseWindow.SetActive(false);
        EventPlanner.PostponeAnEvent(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name), 1200);
    }
}
