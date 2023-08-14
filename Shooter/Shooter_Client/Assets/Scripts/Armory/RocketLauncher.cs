using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RocketLauncher : Weapon {
    [Tooltip("Визуализатор КД - изображение")]
    [SerializeField] private Image _foreground;
    public override bool TryShoot(out ShootInfo info) {
        if (base.TryShoot(out info)) {
            if (_numberOfBullets != 0) {
                Deactivate();
                Activate();
                ShowShotEffects();
            }
            return true;
        } else {
            info = default;
            return false;
        }
    }

    public override void Activate() {
        base.Activate();
        StartCoroutine(StartCharge());
    }

    private IEnumerator StartCharge() {
        if (_foreground != null) {
            _foreground.gameObject.SetActive(true);
            yield return StartCoroutine(Demonstration());
        }
    }

    public IEnumerator Demonstration() {

        float duration = _shotPeriod;    // Длительность изменения (в секундах)
        float elapsed = 0f;     // Прошедшее время

        float startValue = 1f;
        float currentValue = 1f;
        float endValue = 0f;

        while (elapsed < duration || currentValue != 0f) {
            float t = elapsed / duration;
            currentValue = Mathf.Lerp(startValue, endValue, t);
            _foreground.fillAmount = currentValue;

            elapsed += Time.deltaTime; // Прирост времени
            yield return null; // Переход на следующий кадр
        }
    }

    public override void AddBullets(int numberOfBullets) {
        base.AddBullets(numberOfBullets);
        _numberOfBullets += numberOfBullets;
        UpdateText();
        _playerArmory.SetWeaponID(1);
    }
}
