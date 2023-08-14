using UnityEngine;

public class MoveToTarget : MonoBehaviour {
    [SerializeField] private Transform _target;

    public void Init(Transform target) {
        _target = target;
    }

    void Update() {
        transform.position = _target.position;
    }
}
