using System;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    private Shield _shield;
    private GameObject _shieldBar;

    private void Awake()
    {
        _shield = GetComponentInParent<Shield>();
        _shieldBar = transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
    }
    
    private void Start()
    {
        _shield.OnShieldChange += ChangeBarRotation;
    }

    private void Update()
    {
        if (transform.eulerAngles != Vector3.up)
            transform.eulerAngles = Vector3.up;
    }

    private void ChangeBarRotation()
    {
        _shieldBar.transform.localEulerAngles = new Vector3(0, 0, 45 - 90 * _shield.CurrentShield / _shield.MaxShield);
    }
}
