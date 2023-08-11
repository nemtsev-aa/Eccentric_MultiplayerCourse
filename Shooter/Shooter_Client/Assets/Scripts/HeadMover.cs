using UnityEngine;

public class HeadMover : MonoBehaviour {

    [SerializeField] private Transform _target;

    void Update()
    {
        transform.position = _target.position;
    }
}
