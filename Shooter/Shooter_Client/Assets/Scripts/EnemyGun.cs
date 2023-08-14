using UnityEngine;

public class EnemyGun : Gun {
    [SerializeField] private Armory _armory;
    [SerializeField] private FlashView _flashView;

    public void Shoot(Vector3 position, Vector3 velocity) {
        //Debug.Log($"Пришло с сервера: {position}, {velocity}");
        
        Weapon weapon = _armory.ActiveWeapon;
        _flashView.SetTarget(_armory.CurrentWeaponID);
        Transform[] bulletCreatorPoints = new Transform[0];
        if (weapon.CreatePointsList() != null) {
            bulletCreatorPoints = weapon.CreatePointsList().ToArray();

            foreach (var iPoint in bulletCreatorPoints) {
                Bullet newBullet = Instantiate(weapon.BulletPrefab, iPoint.position, iPoint.rotation);
                newBullet.Init(velocity, weapon.Damage);
            }
            weapon.OnShot?.Invoke();
            weapon.ShowShotEffects();
        }
    }
}
