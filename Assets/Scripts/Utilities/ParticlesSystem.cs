using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Components
{
    public class ParticlesSystem : MonoBehaviour
    {
        [SerializeField] private ParticleSystem Part;
        
        
        /// <summary>
        /// Метод создает копию шаблонной системы частиц, изменяет переданные параметры и запускает систему частиц
        /// </summary>
        /// <param name="power"> Мощность взрыва </param>
        /// <param name="position"> Позиция запуска системы частиц </param>
        public void PlayNewParticles(int power, Vector2 position)
        {
            var part = Instantiate(Part);
            var mainModule = part.main;
            var emissionModule = part.emission;
            part.transform.position = position;
            mainModule.startSpeed = power * 30;
            mainModule.startSize = (float)(power * 0.35);
            mainModule.startLifetime = (float)(power * 0.75);
            emissionModule.burstCount = power * 50;
            part.Play();
            Destroy(part.gameObject, 1);
        }
    }
}