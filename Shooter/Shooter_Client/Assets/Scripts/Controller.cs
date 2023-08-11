using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private PlayerGun _gun;
    [SerializeField] private Squat _squat;
    [SerializeField] private float _mouseSensetivity = 2f;
    private MultiplayerManager _multiplayerManager;

    private void Start() {
        _multiplayerManager = MultiplayerManager.Instance;
    }

    private void Update() {
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
        if (isShoot && _gun.TryShoot(out ShootInfo shootInfo)) SendShoot(ref shootInfo);

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
}

[System.Serializable]
public struct ShootInfo {
    public string key;
    public float pX;
    public float pY;
    public float pZ;
    public float dX;
    public float dY;
    public float dZ;
}
