using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image image;
    
    void Start()
    {
        image.sprite = sprites[
            Random.Range(0, sprites.Length)
        ];
    }
}
