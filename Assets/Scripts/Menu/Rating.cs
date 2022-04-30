using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rating : MonoBehaviour
{
    public GameObject[] states;
    private int rating;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Rating"))
            PlayerPrefs.SetInt("Rating", 0);
    }

    void Update()
    {
        var newRating = PlayerPrefs.GetInt("Rating");
        if (newRating > rating)
        {
            states[rating].SetActive(false);
            states[newRating].SetActive(true);
            rating = newRating;
        }
    }

    public void ClearRating()
    {
        PlayerPrefs.DeleteAll();
        
        foreach (var state in states)
            state.SetActive(false);
        states[0].SetActive(true);

        rating = 0;
    }
}
