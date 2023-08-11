using UnityEngine;

public class PlayerCharacter : Character {
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] Transform _head;
    [SerializeField] float _minHeadAngle = -90f;
    [SerializeField] float _maxHeadAngle = 90f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private float _jumpDelay = 0.2f;

    private float _inputH;
    private float _inputV;
    private float _rotateX;
    private float _rotateY;
    private float _currentRotateX;
    private float _jumpTime;


    private void Start() {
        Transform camera = Camera.main.transform;
        camera.parent = _cameraPoint;
        camera.localPosition = Vector3.zero;
        camera.localRotation = Quaternion.identity;
    }

    private void Update() {
        RotateX(_rotateX);
    }

    private void FixedUpdate() {
        Move();
        RotateY();
    }

    public void SetInput(float h, float v, float rotateX, float rotateY) {
        _inputH = h;
        _inputV = v;
        _rotateX = rotateX;
        _rotateY += rotateY;
    }

    private void Move() {
        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * Speed;
        velocity.y = _rigidbody.velocity.y;
        Velocity = velocity;
        _rigidbody.velocity = Velocity;
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY) {
        position = transform.position;
        velocity = _rigidbody.velocity;
        rotateX = _head.localEulerAngles.x;
        rotateY = transform.eulerAngles.y;
    }

    public void RotateX(float value) {
        _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
        _head.localEulerAngles = new Vector3(_currentRotateX, 0, 0);
    }

    private void RotateY() {
        _rigidbody.angularVelocity = new Vector3(0f, _rotateY, 0f);
        _rotateY = 0;
    }

    public void Jump() {
        if (_checkFly.IsFly) return;
        if (Time.time - _jumpTime < _jumpDelay) return;

        _jumpTime = Time.time;
        _rigidbody.AddForce(0, _jumpForce, 0, ForceMode.VelocityChange);
    }
}
