using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SoundAdapter : MonoBehaviour
{
    [SerializeField] private List<string> soundNames;
    [SerializeField] private List<AudioSource> soundSources;
    public Dictionary<string, AudioSource> sounds;

    private Engine _engine;
    private Health _health;
    private WeaponController _weaponController;
    private CoolDown _coolDown;

    private void Awake()
    {
        _engine = GetComponent<Engine>();
        _health = GetComponent<Health>();
        _weaponController = GetComponentInChildren<WeaponController>();
        _coolDown = GetComponentInChildren<CoolDown>(); //TODO: получить кулдаун действующего оружия

        sounds = new Dictionary<string, AudioSource>();
        for (var i = 0; i < soundNames.Count; i++)
            sounds[soundNames[i]] = soundSources[i];
    }

    private void Start()
    {
        if (sounds.TryGetValue("death", out var deathSound))
            _health.OnDeath += () => { PlaySound(deathSound); };
        if (sounds.TryGetValue("shoot", out var shootSound))
            _weaponController.OnShoot += _ => PlaySound(shootSound, _coolDown);
        if (sounds.TryGetValue("move", out var moveSound))
            _engine.OnBoost += _ => PlaySound(moveSound);
    }

    private static void PlaySound(AudioSource state)
    {
        if (!state.isPlaying)
            state.Play();
    }

    private static void PlaySound(AudioSource state, CoolDown coolDown)
    {
        if (!state.isPlaying)
            state.Play();
    }
}
