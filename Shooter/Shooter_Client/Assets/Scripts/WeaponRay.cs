using UnityEngine; 

[ExecuteInEditMode]
public class WeaponRay : MonoBehaviour {
    public bool Status => _status;
    [SerializeField] private bool _status = true;
    [SerializeField] private Armory _armory;
    [SerializeField] private MoveToTarget _moveToTarget;

    [SerializeField] private Transform _pointer;
    [SerializeField] private float _pointSize;
    [SerializeField] private Transform _center;
    [SerializeField] private float _distance = 50f;
    [SerializeField] private LayerMask _layerMask;
    
    private Transform _camera;

    private void Awake() {
        _camera = Camera.main.transform;
        _armory.OnActiveWeaponChanged += SetStartRay;
        SetActive(_status);
    }

    void Update() {
        if (_status) {
            Ray ray = new Ray(_center.position, _center.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, _distance, _layerMask, QueryTriggerInteraction.Ignore)) {
                _center.localScale = new Vector3(1, 1, hit.distance);
                _pointer.position = hit.point;
                float distance = Vector3.Distance(_center.position, hit.point);
                _pointer.localScale = Vector3.one * _pointSize * distance;
            }
        }
    }

    public void SetActive(bool status) {
        _status = status;
        _center.gameObject.SetActive(status);
    }

    public void SetStartRay(int weaponID) {
        _moveToTarget.Init(_armory.ActiveWeapon.BulletCreator);
    }

    private void OnDisable() {
        _armory.OnActiveWeaponChanged -= SetStartRay;
    }

    private void OnDrawGizmosSelected() {
        Ray ray = new Ray(_center.position, _center.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);
    }
}
