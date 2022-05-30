﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utilities
{
    public class InGameDialogue : MonoBehaviour
    {
        [SerializeField] private Text startDialog;
        [SerializeField] private string[] text;
        [SerializeField] private float textSpeed;
        [SerializeField] private AudioSource typingSound;
        
        private int _index;
        void Start()
        {
            startDialog.text = string.Empty;
            _index = 0;
            StartDialog();
        }
    
        void Update()
        {
            if (!Input.anyKeyDown) return;
            
            if (startDialog.text == text[_index])
            {
                IsNextLine();
            }
            else
            {
                TurnExtensions(false);
                StopAllCoroutines();
                startDialog.text = text[_index];
            }
        }
    
        void StartDialog()
        {
            _index = 0;
            StartCoroutine(TypeLine());
        }
    
        IEnumerator TypeLine()
        {
            TurnExtensions(true);
    
            foreach(char c in text[_index])
            {
                startDialog.text += c;
                yield return new WaitForSeconds(1 / textSpeed);
            }
            TurnExtensions(false);
        }
        
        void IsNextLine()
        {
            if(_index < text.Length -1)
            {
                _index++;
                startDialog.text = string.Empty;
                StartCoroutine(TypeLine());
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    
        private void TurnExtensions(bool condition)
        {
            if (condition) typingSound.Play();
            else typingSound.Stop(); 
        }
    }
}