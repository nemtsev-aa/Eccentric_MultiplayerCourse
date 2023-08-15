using UnityEngine;

public class Health : MonoBehaviour {
    public int Max => _max;
    public int Current => _current;

    [SerializeField] private HealthUI _healthUI;
    private int _max;
    private int _current;

    public void SetMax(int max) {
        _max = max;
        UpdateHP();
    }

    public void SetCurrent(int current) {
        _current = current;
        UpdateHP();
    }

    public void ApplyDamage( int damage) {
        _current -= damage;
        UpdateHP();
    }

    private void UpdateHP() {
        _healthUI.UpdateHealth(_max, _current);
    }

    public void AddHealth(int lootValue) {
        _current += lootValue;
        if (_current > _max) _current = _max;
        UpdateHP();
    }
}
