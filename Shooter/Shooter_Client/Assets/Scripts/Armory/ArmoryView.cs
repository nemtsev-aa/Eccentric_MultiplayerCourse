using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ArmoryView : MonoBehaviour {
    [SerializeField] private Armory _armory;
    [Tooltip("���������� ���� - �����")]
    [SerializeField] private TextMeshProUGUI _bulletsCountText;
    [Tooltip("������������� ������ - ������")]
    [SerializeField] private Toggle[] _weaponsIcon;

    void Start() {
        foreach (Weapon iWeapon in _armory.Weapons) {
            iWeapon.Init(_armory, _bulletsCountText);
        }
        _armory.OnActiveWeaponChanged += UpdateView;
    }

    private void UpdateView(int index) {
        _weaponsIcon[index].isOn = true;
    }
}
