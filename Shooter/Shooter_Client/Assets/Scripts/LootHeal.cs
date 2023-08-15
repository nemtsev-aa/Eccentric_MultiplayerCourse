using UnityEngine;

public class LootHeal : MonoBehaviour
{
    [Tooltip("���������� �������� � ��������")]
    [SerializeField] private int _lootValue;

    private void OnTriggerEnter(Collider other) {
        
        if (other.attachedRigidbody.TryGetComponent(out Health health)) {
            Debug.Log("LootHeal: OnTriggerEnter");
            // ���� ����� �� ��������� ������ - ������� ��� �������
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
