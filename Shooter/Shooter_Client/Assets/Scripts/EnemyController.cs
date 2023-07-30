using Colyseus.Schema; 
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    internal void OnChange(List<DataChange> changes) {
        Vector3 position = transform.position;

        foreach (var dataChange in changes) {
            switch (dataChange.Field) {
                case "x":
                    position.x = (float)dataChange.Value;
                    break;
                case "y":
                    position.z = (float)dataChange.Value;
                    break;

                default:
                    Debug.LogWarning("�� �������������� ��������� ���� " + dataChange.Field);
                    break;
            }
        }

        transform.position = position;
    }
}
