using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckForWin : MonoBehaviour
{
    [SerializeField] public Text result;
    [SerializeField] public GameObject victoryWindow;
    [SerializeField] public GameObject[] stations;
    [SerializeField] public GameObject[] ratings;

    private float startTime;
    private float gameTime;

    private void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (stations.Any(station => station.activeSelf))
        {
            return;
        }

        GetEpisodeResult();
    }

    private void GetEpisodeResult()
    {
        if (gameTime < 1e-9)
        {
            gameTime = Time.time - startTime;
            Time.timeScale = 0.2f;
            var stars = 1;

            if (gameTime < 3*60) stars = 3;
            else if (gameTime < 5*60) stars = 2;
            
            result.text = gameTime.ToString(CultureInfo.InvariantCulture);
            victoryWindow.SetActive(true);
            ratings[stars].SetActive(true);
            
            PlayerPrefs.SetInt("Rating", Max(stars, PlayerPrefs.GetInt("Rating")));
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
        SceneManager.LoadScene(1);
    }
}
