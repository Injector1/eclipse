using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class DangerZone : MonoBehaviour
{
    public GameObject player;
    public GameObject warningFrame;
    public GameObject loseFrame;
    public Text warningTime;
    private bool isDead = false;

    private float _startTime;
    private const float MaxTime = 10;
    private const float MaxDistance = 8000;

    private void KillPlayer()
    {
        isDead = true;
        player.SetActive(false);
        Time.timeScale = 02f;
        loseFrame.SetActive(true);
    }

    private string GetStringTime(float time)
    {
        return time.ToString(CultureInfo.InvariantCulture).Substring(0, 5);
    }
    
    void Update()
    {
        if (isDead) return;
        var position = player.transform.position;
        var distanceFromCenter = position.x * position.x + position.y * position.y;

        //Debug.Log(distanceFromCenter);
        if (distanceFromCenter < MaxDistance)
        {
            warningFrame.SetActive(false);
            return;
        }
        
        var currentTime = Time.time - _startTime;

        if (!warningFrame.activeSelf)
        {
            _startTime = Time.time;
            warningFrame.SetActive(true);    
        }
        
        else if (MaxTime - currentTime < 1e-9)
        {
            warningFrame.SetActive(false);
            KillPlayer();
        }

        warningTime.text = GetStringTime(MaxTime - currentTime);
    }
}
