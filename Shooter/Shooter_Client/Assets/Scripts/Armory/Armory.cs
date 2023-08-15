using System;
using System.Collections;
using UnityEngine;


public class Armory : MonoBehaviour {
    public Weapon ActiveWeapon => _activeWeapon;
    public int CurrentWeaponID => _currentGunIndex;
    public IEnumerable Weapons => _weapons;

    public Action<int> OnActiveWeaponChanged;

    [Tooltip("Список оружия")]
    [SerializeField] private Weapon[] _weapons;
    
    private int _currentGunIndex = 0;
    private Weapon _activeWeapon;

    private void Start() {
        TakeGunByIndex(_currentGunIndex);
    }

    public void SetWeaponID(int id) {
        TakeGunByIndex(id);
    }
    
    public void SetWeaponID(bool value) {
        if (value) {
            if (_currentGunIndex == _weapons.Length-1) _currentGunIndex = 0;
            else _currentGunIndex++;
        } else {
            if (_currentGunIndex == 0) _currentGunIndex = _weapons.Length-1;
            else _currentGunIndex--;
        }
        TakeGunByIndex(_currentGunIndex);
    }

    private void TakeGunByIndex(int gunIndex) {
        foreach (Weapon iWeapon in _weapons) {
            iWeapon.Deactivate();
        }
        _currentGunIndex = gunIndex;
        _activeWeapon = _weapons[_currentGunIndex];
        _activeWeapon.Activate();
        _activeWeapon.HideFlash();

        OnActiveWeaponChanged?.Invoke(_currentGunIndex);
    }

    public void AddBullets(int gunIndex, int numberOfBullets) {
        _weapons[gunIndex].AddBullets(numberOfBullets);
        MultiplayerManager.Instance.GetPlayerCharacter().InfoView.ShowInfoMessage(InfoType.AddAmmo);
    }
}
