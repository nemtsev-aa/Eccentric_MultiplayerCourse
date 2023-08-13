using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    [SerializeField] private float _restartDelay = 3f;
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private Armory _playerArmory;
    [SerializeField] private Squat _squat;
    [SerializeField] private float _mouseSensetivity = 2f;
    private MultiplayerManager _multiplayerManager;
    private bool _hold = false;

    private void Start() {
        _multiplayerManager = MultiplayerManager.Instance;
        _playerArmory.OnActiveWeaponChanged += SendNewWeaponID;
    }

    private void Update() {
        if (_hold) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _player.SetInput(h, v, -mouseY * _mouseSensetivity, mouseX * _mouseSensetivity);

        bool space = Input.GetKeyDown(KeyCode.Space);
        if (space) _player.Jump();

        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            _squat.SetSquatState(true);
            SendSquat();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl)) {
            _squat.SetSquatState(false);
            SendSquat();
        }

        bool isShoot = Input.GetMouseButton(0);
        if (isShoot && _playerArmory.ActiveWeapon.TryShoot(out ShootInfo shootInfo)) SendShoot(ref shootInfo);

        SendWeaponID();
        SendMove();
    }

    private void SendShoot(ref ShootInfo shootInfo) {
        shootInfo.key = _multiplayerManager.GetSessionID();
        string json = JsonUtility.ToJson(shootInfo);
        _multiplayerManager.SendMessage("shoot", json);
    }

    private void SendMove() {
        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY);
        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"pX", position.x},
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", velocity.x},
            {"vY", velocity.y},
            {"vZ", velocity.z},
            {"rX", rotateX },
            {"rY", rotateY }
        };
        _multiplayerManager.SendMessage("move", data);
    }

    private void SendSquat() {

        Dictionary<string, object> data = new Dictionary<string, object>(){
            { "sq", _squat.IsSquating },
        };
        _multiplayerManager.SendMessage("squat", data);
    }

    public void Restart(string jsonRestartInfo) {
        RestartInfo info = JsonUtility.FromJson<RestartInfo>(jsonRestartInfo);

        StartCoroutine(Hold());

        _player.transform.position = new Vector3(info.x, 0f, info.z);
        _player.SetInput(0, 0, 0, 0);

        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY);
        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"pX", info.x},
            {"pY", 0},
            {"pZ", info.z},
            {"vX", 0},
            {"vY", 0},
            {"vZ", 0},
            {"rX", 0},
            {"rY", 0}
        };
        _multiplayerManager.SendMessage("move", data);
    }

    private IEnumerator Hold() {
        _hold = true;
        yield return new WaitForSecondsRealtime(_restartDelay);
        _hold = false;
    }

    private void SendWeaponID() {
        //int currentID = _playerArmory.CurrentWeaponID;

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            _playerArmory.SetWeaponID(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            _playerArmory.SetWeaponID(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            _playerArmory.SetWeaponID(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            _playerArmory.SetWeaponID(3);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") >= 0.1f) {
            _playerArmory.SetWeaponID(true);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
            _playerArmory.SetWeaponID(false);
        }
    }

    private void SendNewWeaponID(int value) {
        Dictionary<string, object> data = new Dictionary<string, object>() { { "wID", value } };
        _multiplayerManager.SendMessage("wID", data);
    }

    private void OnDisable() {
        _playerArmory.OnActiveWeaponChanged -= SendNewWeaponID;
    }
}

[System.Serializable]
public struct ShootInfo {
    public string key;
    public int weaponID;
    public float pX;
    public float pY;
    public float pZ;
    public float dX;
    public float dY;
    public float dZ;
}

[System.Serializable]
public struct RestartInfo {
    public float x;
    public float z;
}
