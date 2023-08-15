using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour {
    private const string _damage = "Damage";
    private const string _healing = "Healing";
    [SerializeField] private RectTransform _filledImage;
    [SerializeField] private float _defaultWidth;
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI _damageValueText;

    private int _pastHealth;

    private void OnValidate() {
        _defaultWidth = _filledImage.sizeDelta.x;
    }

    public void UpdateHealth(float max, int current) {

        float percent = current / max;
        _filledImage.sizeDelta = new Vector2(_defaultWidth * percent, _filledImage.sizeDelta.y);

        int value = 0;
        if (_pastHealth > current) {
            value = _pastHealth - current;
            if (_damageValueText != null) _damageValueText.text = $"-{value}";           
            _animator.SetTrigger(_damage);
        } else {
            value = current - _pastHealth;
            if (_damageValueText != null) _damageValueText.text = $"+{value}";
            _animator.SetTrigger(_healing);
        }
        _pastHealth = current;
    }
}
