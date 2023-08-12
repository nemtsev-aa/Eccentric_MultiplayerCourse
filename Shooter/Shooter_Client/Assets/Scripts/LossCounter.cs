using TMPro;
using UnityEngine;

public class LossCounter : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _lossCountText;

    private int _enemyLoss;
    private int _playerLoss;

    public void SetEnemyLoss(int value) {
        _enemyLoss = value;
        UpdateText();
    }

    public void SetPlayerLoss(int value) {
        _playerLoss = value;
        UpdateText();
    }

    private void UpdateText() {
        _lossCountText.text = $"{_playerLoss} : {_enemyLoss}";
    }
}
