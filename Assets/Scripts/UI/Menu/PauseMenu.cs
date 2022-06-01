using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private static bool _isPaused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject loseFrame;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
        switch (_isPaused)
        {
            case true:
                ResumeGame();
                break;
            case false:
                PauseGame();
                break;
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void LoadMenu()
    {
        ResumeGame();
        SceneManager.LoadScene(1);
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        loseFrame.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
