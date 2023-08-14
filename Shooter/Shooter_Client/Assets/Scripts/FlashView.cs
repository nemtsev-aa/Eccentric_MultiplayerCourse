using System;
using UnityEngine;

public class FlashView : MonoBehaviour {
    [SerializeField] private Armory _armory;
    [SerializeField] private MoveToTarget _moveToTarget;
    [SerializeField] private GameObject _flash;

    public void Start() {
        _armory.OnActiveWeaponChanged += SetTarget;
        SetActive(false);
    }

    public void SetActive(bool status) {
        _flash.SetActive(status);
    }

    public void SetTarget(int weaponID) {
        _moveToTarget.Init(_armory.ActiveWeapon.BulletCreator);
    }

    private void OnDisable() {
        _armory.OnActiveWeaponChanged -= SetTarget;
    }
}
