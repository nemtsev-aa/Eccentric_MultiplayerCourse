using System;
using UnityEngine;

public class PlayerGun : MonoBehaviour {

    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _bulletCreator;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _shootDelay;
    private float _lastShootTime;
    public Action OnShoot;

    public bool TryShoot(out ShootInfo info) {
        info = new ShootInfo();
        if (Time.time - _lastShootTime < _shootDelay) return false;

        Vector3 position = _bulletCreator.position;
        Vector3 direction = _bulletCreator.forward;

        _lastShootTime = Time.time;
        Instantiate(_bullet, _bulletCreator.position, _bulletCreator.rotation).Init(_bulletCreator.forward, _bulletSpeed);
        OnShoot?.Invoke();

        direction *= _bulletSpeed;
        info.pX = position.x;
        info.pY = position.y;
        info.pZ = position.z;
        info.dX = direction.x;
        info.dY = direction.y;
        info.dZ = direction.z;

        return true;
    }
}
