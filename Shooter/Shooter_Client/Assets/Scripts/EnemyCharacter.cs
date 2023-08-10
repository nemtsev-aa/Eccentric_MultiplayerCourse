using UnityEngine;

public class EnemyCharacter : MonoBehaviour {

    public Vector3 TargetPosition { get; private set; } = Vector3.zero;
    private float _velocityMagnitude = 0f;

    private void Start() {
        TargetPosition = transform.position;
    }

    private void Update() {
        if (_velocityMagnitude > 0.1f) {
            float maxDistance = _velocityMagnitude * Time.deltaTime;                // Максимальная дистанция, которую может пройти враг за единицу времени
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, maxDistance);
        } else {
            transform.position = TargetPosition;
        }
    }

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval) {
        // Предполагаемая позиция врага, зависящая от направления его движения и среднего времени получения данных с сервера
        TargetPosition = position + (velocity * averageInterval);
        _velocityMagnitude = velocity.magnitude;
    }
}
