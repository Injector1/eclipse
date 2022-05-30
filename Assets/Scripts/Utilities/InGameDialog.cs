using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utilities
{
    public class InGameDialog : MonoBehaviour
    {
        [SerializeField] private GameObject dialogFrame;
        [SerializeField] private GameObject[] speakers;
        [SerializeField] private Text dialogText;
        [SerializeField] private AudioSource typingSound;
        
        private int _frameIndex;
        private string[] _text;
        private float _textSpeed;
        private float _breakTime;
        private int _nowSpeaking;
        private bool _isDialogPlaying;

        void Update()
        {
            if (!_isDialogPlaying) return;
            if (!Input.anyKeyDown) return;
            
            if (dialogText.text == _text[_frameIndex].Replace("#", string.Empty))
            {
                IsNextLine();
            }
            else
            {
                TurnExtensions(false);
                StopAllCoroutines();
                dialogText.text = _text[_frameIndex].Replace("#", string.Empty);
            }
        }
        
        public void StartDialog(string text, int personIndex, float textSpeed)
        {
            dialogText.text = string.Empty;
            _frameIndex = 0;
            _breakTime = 10000;
            _nowSpeaking = personIndex;
            _text = text.Split('&');
            _textSpeed = textSpeed * 50000;
            
            PrepareScene(true);
            StartCoroutine(TypeLine());
        }
    
        IEnumerator TypeLine()
        {
            TurnExtensions(true);
    
            foreach(char c in _text[_frameIndex])
            {
                if (c != '#') dialogText.text += c;
                yield return new WaitForSeconds(c == '#' ? 1 / _breakTime : 1 / _textSpeed);
            }
            TurnExtensions(false);
        }
        
        void IsNextLine()
        {
            if(_frameIndex < _text.Length -1)
            {
                _frameIndex++;
                dialogText.text = string.Empty;
                StartCoroutine(TypeLine());
            }
            else
            {
                PrepareScene(false);
            }
        }
    
        private void TurnExtensions(bool condition)
        {
            if (condition) typingSound.Play();
            else typingSound.Stop(); 
        }

        private void PrepareScene(bool turned)
        {
            Time.timeScale = turned ? 0.0001f : 1f;
            dialogFrame.SetActive(turned);
            speakers[_nowSpeaking].SetActive(turned);
            _isDialogPlaying = turned;
        }
    }
}