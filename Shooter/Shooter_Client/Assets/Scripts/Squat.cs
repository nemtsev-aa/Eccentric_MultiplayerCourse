using UnityEngine;

public class Squat : MonoBehaviour {
    public bool IsSquating { get; private set; }
    [SerializeField] private Transform _body;
    [SerializeField] private Character _character;
    private float _defaultScaleZ;

    private void Start() {
        _defaultScaleZ = transform.localScale.z;
    }

    private void Update() {
        if (!IsSquating) SetBodyScale(_defaultScaleZ);
        else SetBodyScale(_defaultScaleZ / 2f);
    }

    private void SetBodyScale(float value) {
        Vector3 newBodyScale = new Vector3(_body.localScale.x, _body.localScale.y, value);
        _body.localScale = Vector3.Lerp(_body.localScale, newBodyScale, Time.deltaTime * _character.SquatingSpeed);
    }

    public void SetSquatState(bool value) => IsSquating = value;
}
