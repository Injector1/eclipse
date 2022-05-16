using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEntry : MonoBehaviour
{
    [SerializeField] public GameObject[] frames;
    [SerializeField] private int[] frameTime;
    [SerializeField] private Image sprite;
    private const int PauseTime = 2000;  
    
    void Start()
    {
        SmoothTransit();
    }

    private async void SmoothTransit()
    {
        await Task.Delay(1000);

        for (int frameIndex = 0; frameIndex < frames.Length; frameIndex++)
        {
            frames[frameIndex].SetActive(true);
            ShowText();

            await Task.Delay(frameTime[frameIndex]);
            HideText();
            await Task.Delay(PauseTime);

            frames[frameIndex].SetActive(false);
        }

        SceneManager.LoadScene(1);
    }
    
    private void HideText()
    {
        StartCoroutine(nameof(InvisibleText));
    }

    private void ShowText()
    {
        StartCoroutine(nameof(VisibleText));
    }
    
    IEnumerator InvisibleText()
    {
        for (float f = 0.05f; f <= 1; f += 0.01f)
        {
            ModifyColor(f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator VisibleText()
    {
        for (float f = 1f; f >= -0.05f; f -= 0.01f)
        {
            ModifyColor(f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void ModifyColor(float f)
    {
        var color = sprite.color;
        color.a = f;
        sprite.color = color;
    }
}
