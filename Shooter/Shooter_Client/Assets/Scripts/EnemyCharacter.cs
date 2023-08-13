using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character {
    [SerializeField] private Health _health;
    [SerializeField] private Transform _head;

    public Vector3 TargetPosition { get; private set; } = Vector3.zero;
    [SerializeField] float _rotationSpeed = 15f;
    private float _velocityMagnitude = 0f;
    private Vector3 _localEulerAnglesX;
    private Vector3 _localEulerAnglesY;
    private string _sessionID;

    private void Start() {
        TargetPosition = transform.position;
    }

    public void Init(string sessionID) {
        _sessionID = sessionID;
    }

    public void SetMaxHP(int value) {
        MaxHealth = value;
        _health.SetMax(value);
        _health.SetCurrent(value);
    }

    public void RestoreHP(int newValue) {
        _health.SetCurrent(newValue);
    }

    public void SetSpeed(float value) => Speed = value;
    public void SetSpeedSquat(float value) => SquatingSpeed = value;
    
    private void Update() {
        if (_velocityMagnitude > 0.1f) {
            float maxDistance = _velocityMagnitude * Time.deltaTime;                // Максимальная дистанция, которую может пройти враг за единицу времени
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, maxDistance);
        } else {
            transform.position = TargetPosition;
        }
        _head.localRotation = Quaternion.Lerp(_head.localRotation, Quaternion.Euler(_localEulerAnglesX), Time.deltaTime *_rotationSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(_localEulerAnglesY), Time.deltaTime * _rotationSpeed);
    }

    public void ApplyDamage(int damage) {
        _health.ApplyDamage(damage);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"id", _sessionID },
            {"value", damage }
        };

        MultiplayerManager.Instance.SendMessage("damage", data);
    }

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval) {
        // Предполагаемая позиция врага, зависящая от направления его движения и среднего времени получения данных с сервера
        TargetPosition = position + (velocity * averageInterval);
        _velocityMagnitude = velocity.magnitude;

        Velocity = velocity;
    }

    public void SetRotateX(float value) {
        _localEulerAnglesX = new Vector3(value, 0, 0);
    }

    public void SetRotateY(float value) {
        _localEulerAnglesY = new Vector3(0, value, 0);
    }
}
