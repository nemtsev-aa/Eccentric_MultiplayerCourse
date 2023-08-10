using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private float _mouseSensetivity = 2f;


    private void Update() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        bool space = Input.GetKeyDown(KeyCode.Space);

        _player.SetInput(h,v, mouseX * _mouseSensetivity);
        _player.RotateX(-mouseY * _mouseSensetivity);
        if (space) {
            _player.Jump();
        }
        SendMove();
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
        MultiplayerManager.Instance.SendMessage("move", data);
    }
}
