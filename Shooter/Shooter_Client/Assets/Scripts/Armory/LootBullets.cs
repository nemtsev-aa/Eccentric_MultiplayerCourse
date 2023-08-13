using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBullets : MonoBehaviour
{
    [Tooltip("������ �������� ������")]
    [SerializeField] private int _gunIndex;
    [Tooltip("���������� �������� � ��������")]
    [SerializeField] private int _lootValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.TryGetComponent(out Armory playerArmory))
        {
            Take(playerArmory);
        }
    }

    public void Take(Armory playerArmory)
    {
        playerArmory.AddBullets(_gunIndex, _lootValue);
        Destroy(gameObject);
    }
}
