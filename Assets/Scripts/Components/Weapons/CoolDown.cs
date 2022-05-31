using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CoolDown : MonoBehaviour
{
    private bool isCoolDown;
    [SerializeField] public float CoolDownInSec;

    public bool TryToShoot()
    {
        if (isCoolDown)
            return false;
        isCoolDown = true;
        this.StartCoroutine(() => isCoolDown = false, CoolDownInSec);
        return true;
    }
}