using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Components
{
    public class ParticlesSystem : MonoBehaviour
    {
        [SerializeField] private ParticleSystem part;
        
        
        /// <summary>
        /// Метод создает копию шаблонной системы частиц, изменяет переданные параметры и запускает систему частиц
        /// </summary>
        /// <param name="power"> Мощность взрыва </param>
        /// <param name="position"> Позиция запуска системы частиц </param>
        public void PlayNewParticles(int power, Vector2 position)
        {
            var particles = Instantiate(part);
            var mainModule = particles.main;
            var emissionModule = particles.emission;
            particles.transform.position = position;
            mainModule.startSpeed = power * 3;
            mainModule.startSize = (float)(power * 0.3);
            mainModule.startLifetime = (float)(power * 0.05);
            emissionModule.burstCount = power * 5;
            particles.Play();
            Destroy(particles.gameObject, 1);
        }
    }
}