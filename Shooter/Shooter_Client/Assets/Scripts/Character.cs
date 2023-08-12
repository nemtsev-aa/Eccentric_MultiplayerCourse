using UnityEngine;

public abstract class Character : MonoBehaviour {
    [field: SerializeField] public int MaxHealth { get; protected set; } = 10;
    [field: SerializeField] public float Speed { get; protected set; } = 10f;
    [field: SerializeField] public float SquatingSpeed { get; protected set; } = 30f;
    public Vector3 Velocity { get; protected set; }
}
