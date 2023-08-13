using UnityEngine;

public class GunAnimation : MonoBehaviour {
    private const string _shoot = "Shoot";
    [SerializeField] private Armory _armory;
    [SerializeField] private Animator _animator;

    private void Start() {
        foreach (Weapon iWeapon in _armory.Weapons) {
            iWeapon.OnShot += Shoot;
        }
    }

    private void Shoot() {
        _animator.SetTrigger(_shoot);
    }

    private void OnDestroy() {
        foreach (Weapon iWeapon in _armory.Weapons) {
            iWeapon.OnShot -= Shoot;
        }
    }
}
