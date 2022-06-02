using UnityEngine;
using UnityEngine.Video;

namespace Components.Episode_1
{
    public class GameIntro : MonoBehaviour
    {
        [SerializeField] private GameObject video;
        [SerializeField] private GameObject intro;
        
        void Start()
        {
            video.SetActive(true);
        }

        void Update()
        {
            if (!video.activeSelf && !intro.activeSelf)
            {
                video.SetActive(false);
                //intro.SetActive(true);
            }
                
        }
    }
}
