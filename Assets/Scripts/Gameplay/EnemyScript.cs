using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //[SerializeField] private int damage = 10; // ����, ������� ������� ���� ��� ������������

    private void OnTriggerEnter(Collider other)
    {
        // ���������, ���� �� �� ������������� ������� ������ PlayerMarker
        PlayerMarker playerMarker = other.GetComponent<PlayerMarker>();
        if (playerMarker != null)
        {/*
            // �������� ����� ��������� �������� � ������
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }*/

            // ���������� �����
            Destroy(gameObject);
        }
    }
}