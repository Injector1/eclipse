using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CoolDown : MonoBehaviour
{
    private bool isCoolDown;
    [SerializeField] public int CoolDownInMs;

    public bool TryToShoot()
    {
        if (isCoolDown)
            return false;
        isCoolDown = true;
        Task.Run(() => Wait(CoolDownInMs));
        return true;
    }

    private void Wait(int cdTime)
    {
        if (cdTime < 0)
            return;
        Thread.Sleep(cdTime);
        isCoolDown = false;
    }
}