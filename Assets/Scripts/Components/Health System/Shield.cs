using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    [SerializeField] public float MaxShield;
    [SerializeField] public float CurrentShield;
    [SerializeField] public float SecondsWithoutDamageToRegen;
    [SerializeField] public float ShieldRegenPerSecond;
    public Action OnShieldChange;

    private DateTime _lastDamageTaken;
    private int _tickInMS = 100;
    private float _regenPerTick;
    private bool _regenStopFlag;

    private void Awake()
    {
        CurrentShield = CurrentShield == 0 ? MaxShield : CurrentShield;
        _regenPerTick = ShieldRegenPerSecond * _tickInMS / 1000;
        _regenStopFlag = true;
    }
    
    public void ShieldAdd(float shieldAdd)
    {
        CurrentShield += shieldAdd;
        if (CurrentShield > MaxShield)
        {
            CurrentShield = MaxShield;
            StopRegen();
        }
        OnShieldChange?.Invoke();
    }

    public float TakeDamage(float damage)
    {
        _lastDamageTaken = DateTime.Now;
        StartRegen();
        CurrentShield -= damage;
        var damageLeft = CurrentShield >= 0 ? 0 : -CurrentShield;
        if (CurrentShield < 0)
            CurrentShield = 0;
        OnShieldChange?.Invoke();
        return damageLeft;
    }

    public void StartRegen()
    {
        if (!_regenStopFlag)
            return;
        _regenStopFlag = false;
        Regen();
    }
    
    public void StopRegen()
    {
        _regenStopFlag = true;
    }
    
    private void Regen()
    {
        if (CurrentShield < MaxShield  && (DateTime.Now - _lastDamageTaken).TotalSeconds >= SecondsWithoutDamageToRegen)
            ShieldAdd(_regenPerTick);
        if (!_regenStopFlag)
            ActionPlanner.PostponeAnAction(Regen, _tickInMS);
    }
}