using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum InfoType {
    Kill,
    HeadShoot,
    Restart,
    AddAmmo
}

public class InfoView : MonoBehaviour {
    private const string _show = "Show";
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Image _messageImage;

    [SerializeField] private Sprite _killIcon;
    [SerializeField] private Sprite _headShootIcon;
    [SerializeField] private Sprite _restartIcon;
    [SerializeField] private Sprite _addAmmoIcon;

    [SerializeField] private Animator _animator;

    public void ShowInfoMessage(InfoType type, string text="") {
        switch (type) {
            case InfoType.Kill:
                _messageText.text = $"Убит!";
                _messageImage.sprite = _killIcon;
                Debug.Log("Kill");
                break;
            case InfoType.Restart:
                _messageText.text = $"Возраждение";
                _messageImage.sprite = _restartIcon;
                Debug.Log("Restart");
                break;
            case InfoType.AddAmmo:
                _messageText.text = $"Получены боеприпасы";
                _messageImage.sprite = _addAmmoIcon;
                Debug.Log("AddAmmo");
                break;
            case InfoType.HeadShoot:
                _messageText.text = $"В голову!";
                _messageImage.sprite = _headShootIcon;
                Debug.Log("HeadShoot");
                break;
            default:
                break;
        }

        _animator.enabled = true;
        _animator.SetTrigger("Show");
    }
}
