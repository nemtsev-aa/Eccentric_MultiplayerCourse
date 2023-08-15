using UnityEngine;

public class LootHeal : MonoBehaviour
{
    [Tooltip("Количество здоровья в предмете")]
    [SerializeField] private int _lootValue;

    private void OnTriggerEnter(Collider other) {
        
        if (other.attachedRigidbody.TryGetComponent(out Health health)) {
            Debug.Log("LootHeal: OnTriggerEnter");
            // Если игрок не полностью здоров - передаём ему лечение
            if (health.Current < health.Max) {
                Take(health);
            }
        }
    }

    public void Take(Health Health) {
        Health.AddHealth(_lootValue);
        Destroy(gameObject);
    }
}
