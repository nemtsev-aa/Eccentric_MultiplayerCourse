using Colyseus.Schema; 
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [SerializeField] private EnemyCharacter _enemyCharacter;
    private List<float> _receiveTimeIntervals = new List<float> { 0f, 0f, 0f, 0f, 0f};
    private float _lastReceiveTime = 0f;

    private void SaveReceiveTime() {
        float interval = Time.time - _lastReceiveTime;
        _lastReceiveTime = Time.time;

        _receiveTimeIntervals.Add(interval);
        _receiveTimeIntervals.Remove(0);
    }

    internal void OnChange(List<DataChange> changes) {

        SaveReceiveTime();

        Vector3 position = _enemyCharacter.TargetPosition;
        Vector3 velocity = Vector3.zero;

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
                default:
                    Debug.LogWarning($"{dataChange.Field} not handled");
                    break;
            }
        }
        _enemyCharacter.SetMovement(position, velocity, _receiveTimeIntervals.Average());
    }
}
