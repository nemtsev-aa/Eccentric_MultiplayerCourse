using System;
using UnityEngine;

public class GunAnimation : MonoBehaviour {
    private const string _shoot = "Shoot";
    [SerializeField] private PlayerGun _gun;
    [SerializeField] private Animator _animator;

    private void Start() {
        _gun.OnShoot += Shoot;
    }

    private void Shoot() {
        _animator.SetTrigger(_shoot);
    }

    private void OnDestroy() {
        _gun.OnShoot -= Shoot;
    }
}
