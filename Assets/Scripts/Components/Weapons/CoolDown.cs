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
        ActionPlanner.PostponeAnAction(() => isCoolDown = false, CoolDownInMs);
        return true;
    }
}