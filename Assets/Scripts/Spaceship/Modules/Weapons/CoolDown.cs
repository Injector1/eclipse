using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CoolDown
{
    private bool isCoolDown;
    public int CoolDownInMs;

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
        Thread.Sleep(cdTime);
        isCoolDown = false;
    }

    public CoolDown(int coolDownInMs)
    {
        isCoolDown = false;
        CoolDownInMs = coolDownInMs;
    }
}