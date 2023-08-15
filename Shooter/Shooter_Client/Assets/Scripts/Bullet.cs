using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _lifeTime = 5f;
    [Tooltip("Ёффект попадани€")]
    protected GameObject _hitParticle;
    private int _damage;
    private string _gunslingerID;

    public void Init(Vector3 velocity, int damage = 0, string gunslingerID = "") {
        _damage = damage;
        _gunslingerID = gunslingerID;
        _rigidbody.velocity = velocity;
        StartCoroutine(DelayDestroy());
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.rigidbody == null) {
            Destroy();
            return;
        }

        if (collision.rigidbody.gameObject.TryGetComponent(out EnemyCharacter enemyCharacter)) {
            int damage = 0;
            bool headShot = false;
            if (collision.collider.gameObject.CompareTag("Body")) {
                damage = _damage;
            } else if (collision.collider.gameObject.CompareTag("Head")) {
                damage = _damage * 2;
                headShot = true;
            }
            enemyCharacter.ApplyDamage(damage, _gunslingerID, headShot); 
        }
        Destroy();
    }

    private IEnumerator DelayDestroy() {
        yield return new WaitForSecondsRealtime(_lifeTime);
        Destroy();
    }

    private void Destroy() {
        Destroy(gameObject);
    }
}
