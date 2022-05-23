using System.Threading.Tasks;
using UnityEngine;

public class GunModifier : MonoBehaviour
{
    [SerializeField] private GameObject[] guns;
    [SerializeField] private AudioSource successSound;
    private int _killsCounter;
    private int _currentGunIndex;

    private void Start()
    {
        _killsCounter = 0;
        _currentGunIndex = 0;
    }

    public void ImproveGun()
    {
        _killsCounter++;

        var previousGunIndex = _currentGunIndex;

        if (_killsCounter >= 10) // 10 kills to gun lvl 5 (max)
            _currentGunIndex = 4;
        else if (_killsCounter >= 5) // 5 kills to gun lvl 4
            _currentGunIndex = 3;
        else if (_killsCounter >= 2) // 2 kills to gun lvl 3
            _currentGunIndex = 2;
        else if (_killsCounter >= 1) // 1 kill to gun lvl 2
            _currentGunIndex = 1;

        if (previousGunIndex != _currentGunIndex) ChangeGun(_currentGunIndex);
    }

    private async void ChangeGun(int i)
    {
        await Task.Delay(300);
        successSound.Play();
        Debug.Log($"Improved to new gun {guns[i].name}");
        
        DisableGun(guns[i - 1]);
        guns[i].SetActive(true);
    }

    private static void DisableGun(GameObject oldGun)
    {
        Destroy(oldGun);
    }
}
