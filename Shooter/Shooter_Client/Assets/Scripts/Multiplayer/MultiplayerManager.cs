using Colyseus;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager> {
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private EnemyController _enemyPrefab;
    [field: SerializeField] public Skins Skins { get; private set; }
    [field: SerializeField] public SpawnPoints SpawnPoints { get; private set; }
    [field: SerializeField] public LossCounter LossCounter { get; private set; }
    private ColyseusRoom<State> _room;
    private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();

    protected override void Awake() {
        base.Awake();
        Instance.InitializeClient();
        Connect();
    }

    private async void Connect() {
        SpawnPoints.GetPoint(Random.Range(0, SpawnPoints.Lenght), out Vector3 spawnPosition, out Vector3 spawnRotation);

        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"skins", Skins.Lenght },
            {"points", SpawnPoints.Lenght },
            {"hp", _player.MaxHealth },
            {"speed", _player.Speed },
            {"spSqt", _player.SquatingSpeed },
            {"wID", _player.WeaponID },
            {"pX", spawnPosition.x },
            {"pY", spawnPosition.y },
            {"pZ", spawnPosition.z },
            {"rY", spawnRotation.y }
        };

        _room = await Instance.client.JoinOrCreate<State>("state_handler", data);
        _room.OnStateChange += OnChange;
        _room.OnMessage<string>("Shoot", ApplyShoot);
    }

    private void ApplyShoot(string jsonShootInfo) {
        ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);

        if (_enemies.ContainsKey(shootInfo.key) == false) {
            Debug.LogError("ApplyShoot: зарегистрирован выстрел от несуществующего врага!");
            return;
        }

        _enemies[shootInfo.key].Shoot(shootInfo);
    }

    private void WeaponChanged(string json) {

    }

    private void OnChange(State state, bool isFirstState) {
        if (isFirstState == false) return;
        var player = state.players[_room.SessionId];
        state.players.ForEach((key, player) => {
            if (key == _room.SessionId) CreatePlayer(player);
            else CreateEnemy(key, player);
        });

        _room.State.players.OnAdd += CreateEnemy;
        _room.State.players.OnRemove += RemoveEnemy;
    }

    private void CreatePlayer(Player player) {
        var position = new Vector3(player.pX, player.pY, player.pZ);
        Quaternion rotation = Quaternion.Euler(0,player.rY,0);
        PlayerCharacter playerCharacter = Instantiate(_player, position, rotation);
        player.OnChange += playerCharacter.OnChange;
        _room.OnMessage<int>("Restart", playerCharacter.GetComponent<Controller>().Restart);
        playerCharacter.GetComponent<SetSkin>().SetMaterial(Skins.GetMaterial(player.skin));
    }

    private void CreateEnemy(string key, Player player) {
        var position = new Vector3(player.pX, player.pY, player.pZ);
        var enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
        enemy.Init(key, player);
        enemy.GetComponent<SetSkin>().SetMaterial(Skins.GetMaterial(player.skin));
        _enemies.Add(key, enemy);
    }

    private void RemoveEnemy(string key, Player player) {
        if (_enemies.ContainsKey(key) == false) return;

        var enemy = _enemies[key];
        _enemies.Remove(key);
        
        enemy.Destroy();
    }

    public void SendMessage(string key, Dictionary<string, object> data) {
        _room.Send(key, data);
    }

    public void SendMessage(string key, string data) {
        _room.Send(key, data);
    }

    public string GetSessionID() => _room.SessionId;

    protected override void OnDestroy() {
        base.OnDestroy();
        _room.Leave();
    }
}

