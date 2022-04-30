using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckForWin : MonoBehaviour
{
    [SerializeField] public Text[] counters;
    [SerializeField] public Text result;
    [SerializeField] public GameObject victoryWindow;
    [SerializeField] public GameObject[] ratings;

    private float startTime;
    private float gameTime;

    private void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        foreach (var counter in counters)
        {
            var score = int.Parse(counter.text.Split('/')[0]);
            var maxScore = int.Parse(counter.text.Split('/')[1]);

            if (score != maxScore) return;
        }
        
        GetEpisodeResult();
    }

    private void GetEpisodeResult()
    {
        if (gameTime < 1e-9)
        {
            Time.timeScale = 0.2f;

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
}
