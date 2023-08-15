using UnityEngine;

public class ObjectRotation : MonoBehaviour {
    [Tooltip("Направление вращения")]
    [SerializeField] private Vector3 _rotationVector;

    void Update() {
        transform.Rotate(_rotationVector * Time.deltaTime);
    }
}
