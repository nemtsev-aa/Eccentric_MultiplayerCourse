using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Weapon : MonoBehaviour {
    public Transform BulletCreator => _bulletCreator;

    [Tooltip("Идентификатор оружия")]
    [SerializeField] private int _weaponID;
    [Tooltip("Расположение генератора пуль")]
    [SerializeField] private Transform _bulletCreator;
    [Tooltip("Скорострельность")]
    [SerializeField] protected float _shotPeriod = 0.2f;
    [Tooltip("Оставшееся количество пуль")]
    [SerializeField] protected int _numberOfBullets;

    public Bullet BulletPrefab => _bulletPrafab;
    public int Damage => _damage;

    [Header("Bullet Settings")]
    [Tooltip("Префаб пули")]
    [SerializeField] private Bullet _bulletPrafab;
    [Tooltip("Скорость пули")]
    [SerializeField] private float _speed = 10f;
    [Tooltip("Урон пули")]
    [SerializeField] private int _damage = 10;

    [Header("Effects")]
    [Tooltip("Эффект выстрела")]
    [SerializeField] private FlashView _flash;
    [Tooltip("Звук выстрела")]
    [SerializeField] private AudioSource _shotSound;

    public Action OnShot;

    private float _timer;
    private float _lastShootTime;
    internal bool _isOverUI;

    protected TextMeshProUGUI _bulletsCountText;
    protected Armory _playerArmory;
    private Transform[] _bulletCreatorPoints = new Transform[0];
    private string _gunslingerID;

    public void Init(Armory playerArmory, TextMeshProUGUI bulletsCountText) {
        _playerArmory = playerArmory;
        _bulletsCountText = bulletsCountText;
        _gunslingerID = MultiplayerManager.Instance.GetSessionID();

        if (CreatePointsList() != null) _bulletCreatorPoints = CreatePointsList().ToArray();
    }

    public virtual bool TryShoot(out ShootInfo info) {
        info = new ShootInfo();
        _isOverUI = EventSystem.current.IsPointerOverGameObject();
        if (_isOverUI) {
            return false;
        } else {
            if (Time.time - _lastShootTime < _shotPeriod) return false;
            if (_numberOfBullets == 0) return false;

            _lastShootTime = Time.time;

            Vector3 position = _bulletCreator.position;
            Vector3 velocity = _bulletCreator.forward * _speed;
            info.weaponID = _playerArmory.CurrentWeaponID;
            info.pX = position.x;
            info.pY = position.y;
            info.pZ = position.z;
            info.dX = velocity.x;
            info.dY = velocity.y;
            info.dZ = velocity.z;

            foreach (var iPoint in _bulletCreatorPoints) {
                Bullet newBullet = Instantiate(_bulletPrafab, iPoint.position, iPoint.rotation);
                newBullet.Init(velocity, _damage, _gunslingerID);
                _numberOfBullets--;
            }

            OnShot?.Invoke();
            ShowShotEffects();
            UpdateText();
            return true;
        }
    }

    public List<Transform> CreatePointsList() {
        if (_bulletCreator == null) {
            Debug.LogError("Положение генератора пуль не установлено!");
            return null;
        }
        else {
            List<Transform> childrenList = new List<Transform>();
            if (_bulletCreator.transform.childCount > 1) {
                foreach (Transform child in _bulletCreator) {
                    childrenList.Add(child);
                }
            }
            else {
                childrenList.Add(_bulletCreator);
            }
            return childrenList;
        }
    }

    public void ShowShotEffects() {
        _shotSound.Play();
        if (_flash == null) return;

        _flash.SetActive(true);
        Invoke(nameof(HideFlash), 0.1f);
    }

    public void HideFlash() {
        if (_flash == null) return;
        _flash.SetActive(false);
    }

    public virtual void Activate() {
        gameObject.SetActive(true);
        UpdateText();
    }

    public virtual void Deactivate() {
        gameObject.SetActive(false);
    }

    public virtual void UpdateText() {
        if (_bulletsCountText == null) return;
        _bulletsCountText.text = $"{_numberOfBullets}";
    }

    public virtual void AddBullets(int numberOfBullets) {
        _numberOfBullets += numberOfBullets;
        UpdateText();
    }
}
