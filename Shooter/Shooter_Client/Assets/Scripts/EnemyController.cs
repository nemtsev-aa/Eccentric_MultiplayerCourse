using Colyseus.Schema; 
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [SerializeField] private EnemyCharacter _enemyCharacter;
    [SerializeField] private EnemyGun _enemyGun;
    private List<float> _receiveTimeIntervals = new List<float> { 0f, 0f, 0f, 0f, 0f};
    private float _lastReceiveTime = 0f;
    private Player _player;

    public void Init(Player player) {
        _player = player;
        _enemyCharacter.SetSpeed(player.speed);
        _player.OnChange += OnChange;
    }

    private void SaveReceiveTime() {
        float interval = Time.time - _lastReceiveTime;
        _lastReceiveTime = Time.time;

        _receiveTimeIntervals.Add(interval);
        _receiveTimeIntervals.Remove(0);
    }

    public void Shoot(in ShootInfo info) {
        Vector3 position = new Vector3(info.pX, info.pY, info.pZ);
        Vector3 velocity = new Vector3(info.dX, info.dY, info.dZ);
        _enemyGun.Shoot(position, velocity);
    }

    internal void OnChange(List<DataChange> changes) {

        SaveReceiveTime();

        Vector3 position = _enemyCharacter.TargetPosition;
        Vector3 velocity = _enemyCharacter.Velocity;

        foreach (var dataChange in changes) {
            switch (dataChange.Field) {
                case "pX":
                    position.x = (float)dataChange.Value;
                    break;
                case "pY":
                    position.y = (float)dataChange.Value;
                    break;
                case "pZ":
                    position.z = (float)dataChange.Value;
                    break;
                case "vX":
                    velocity.x = (float)dataChange.Value;
                    break;
                case "vY":
                    velocity.z = (float)dataChange.Value;
                    break;
                case "vZ":
                    velocity.z = (float)dataChange.Value;
                    break;
                case "rX":
                    _enemyCharacter.SetRotateX((float)dataChange.Value);
                    break;
                case "rY":
                    _enemyCharacter.SetRotateY((float)dataChange.Value);
                    break;
                default:
                    Debug.LogWarning($"{dataChange.Field} not handled");
                    break;
            }
        }
        _enemyCharacter.SetMovement(position, velocity, _receiveTimeIntervals.Average());
    }

    public void Destroy() {
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }
}

