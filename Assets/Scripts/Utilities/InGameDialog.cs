using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public class Dialog
    {
        public readonly string Text;
        public readonly int PersonIndex;
        public readonly int TextSpeed;
        
        public Dialog(string t, int i, int tS)
        {
            Text = t;
            PersonIndex = i;
            TextSpeed = tS;
        }    
    }
    
    public class InGameDialog : MonoBehaviour
    {
        [SerializeField] private GameObject helpMessage;
        [SerializeField] private GameObject dialogFrame;
        [SerializeField] private GameObject[] speakers;
        
        [SerializeField] private Text dialogText;
        [SerializeField] private AudioSource typingSound;
        [SerializeField] private AudioSource radioInterference;

        private List<Dialog> _dialogues;
        private int _dialogIndex;
        private bool _isPlaying;

        private string[] _text;
        private float _textSpeed;
        private int _nowSpeaking;
        
        private int _frameIndex;
        private const float BreakTime = 10000;

        private string ReplaceSpecialSymbols(string s)
        {
            return dialogText.text = s
                .Replace("#", string.Empty)
                .Replace("$", string.Empty);
        } 

        void Update()
        {
            if (!_isPlaying || !Input.GetKeyDown(KeyCode.Space)) return;
            
            if (dialogText.text == ReplaceSpecialSymbols(_text[_frameIndex])
                .Replace("#", string.Empty))
            {
                IsNextLine();
            }
            else
            {
                TurnExtensions(false);
                StopAllCoroutines();
                dialogText.text = ReplaceSpecialSymbols(_text[_frameIndex]);
            }
        }

        public void StartDialog(List<Dialog> dialogues)
        {
            _dialogues = dialogues;
            _dialogIndex = 0;
            
            ChangeSpeaker(0);
            PrepareScene(true);
            StartCoroutine(TypeLine());
        }

        IEnumerator TypeLine()
        {
            foreach(char c in _text[_frameIndex])
            {
                if (!typingSound.isPlaying) TurnExtensions(true);

                if (c != '#' && c != '$')
                {
                    dialogText.text += c;
                }
                else
                {
                    typingSound.Stop();
                    if (c == '$')
                    {
                        if (!radioInterference.isPlaying)
                        {
                            typingSound.Stop();
                            radioInterference.Play();
                        }
                        else radioInterference.Stop();
                    }
                }
                yield return new WaitForSeconds(c == '#' ? 1 / BreakTime : 1 / _textSpeed);
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
                if (_dialogIndex + 1 < _dialogues.Count)
                {
                    _dialogIndex += 1;
                    ChangeSpeaker(_dialogIndex);
                    StartCoroutine(TypeLine());
                }
                else PrepareScene(false);
            }
        }
    
        private void TurnExtensions(bool condition)
        {
            if (!condition) radioInterference.Stop();
            helpMessage.SetActive(!condition);
            if (condition) typingSound.Play();
            else typingSound.Stop(); 
        }

        private void PrepareScene(bool turned)
        {
            Time.timeScale = turned ? 0.0001f : 1f;
            dialogFrame.SetActive(turned);
            speakers[_nowSpeaking].SetActive(turned);
            _isPlaying = turned;
        }

        private void ChangeSpeaker(int i)
        {
            dialogText.text = string.Empty;
            _frameIndex = 0;
            speakers[_nowSpeaking].SetActive(false);
            _nowSpeaking = _dialogues[i].PersonIndex;
            speakers[_nowSpeaking].SetActive(true);
            _text = _dialogues[i].Text.Split('&');
            _textSpeed = _dialogues[i].TextSpeed * 50000;
        }
    }
}