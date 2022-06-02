using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckForWin : MonoBehaviour
{
    [SerializeField] public Text result;
    [SerializeField] public GameObject victoryWindow;
    [SerializeField] public GameObject[] ratings;
    
    public void GetEpisodeResult(int rating, int min, int sec)
    {
        Time.timeScale = 0.1f;
        
        result.text = $"{min}:{sec % 60}";
        victoryWindow.SetActive(true);
        ratings[rating].SetActive(true);

        PlayerPrefs.SetInt("Rating", Math.Max(rating, PlayerPrefs.GetInt("Rating")));
    }

    public void LoadMenu()
    {
        victoryWindow.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
