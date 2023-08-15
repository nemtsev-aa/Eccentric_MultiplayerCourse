using UnityEngine;

public class LootBullets : MonoBehaviour {
    [field: SerializeField] public bool Status { get; private set; }
    [SerializeField] private float _delay = 10f;
    [SerializeField] private GameObject _object;

    [Tooltip("Индекс целевого оружия")]
    [SerializeField] private int _gunIndex;
    [Tooltip("Количество патронов в предмете")]
    [SerializeField] private int _lootValue;
    private float _time = 0f;

    private void Start() {
        Status = true;
    }

    private void Update() {
        if (!Status) {
            _time += Time.deltaTime;
            if (_time >= _delay) {
                _time = 0;
                Status = true;
            }
        }
        ShowLoot();
    }

    private void ShowLoot() {
        _object.SetActive(Status);
    }

    private void OnTriggerEnter(Collider other) {
        if (Status) {
            if (other.attachedRigidbody.TryGetComponent(out Controller controller)) {
                Take(controller);
            }
        }
    }

    public void Take(Controller controller) {
        controller.PlayerArmory.AddBullets(_gunIndex, _lootValue);
        Status = false;
    }
}
