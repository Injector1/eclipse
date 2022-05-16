using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private static bool IsPaused = false;
    public GameObject pauseMenu;
    public GameObject loseFrame;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (IsPaused)
            {
                case true:
                    ResumeGame();
                    break;
                case false:
                    PauseGame();
                    break;
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void LoadMenu()
    {
        ResumeGame();
        SceneManager.LoadScene(0);
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        loseFrame.SetActive(false);
        SceneManager.LoadScene(1);
    }
}
