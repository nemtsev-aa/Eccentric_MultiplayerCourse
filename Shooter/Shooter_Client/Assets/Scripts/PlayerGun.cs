using UnityEngine;

public class PlayerGun : Gun {
    [SerializeField] private Transform _bulletCreator;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _shootDelay;
    [SerializeField] private int _damage;
    private float _lastShootTime;
    
    public bool TryShoot(out ShootInfo info) {
        info = new ShootInfo();
        if (Time.time - _lastShootTime < _shootDelay) return false;

        Vector3 position = _bulletCreator.position;
        Vector3 velocity = _bulletCreator.forward * _bulletSpeed;

        _lastShootTime = Time.time;
        Instantiate(_bullet, position, _bulletCreator.rotation).Init(velocity, _damage);
        OnShoot?.Invoke();

        info.pX = position.x;
        info.pY = position.y;
        info.pZ = position.z;
        info.dX = velocity.x;
        info.dY = velocity.y;
        info.dZ = velocity.z;

        return true;
    }
}
