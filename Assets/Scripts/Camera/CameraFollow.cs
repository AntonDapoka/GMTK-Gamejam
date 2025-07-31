using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public bool isTurnOn = true;

    [SerializeField] private Transform targetFollow;  // ������, �� ������� ��������� ������
    [SerializeField] private Transform targetLook;    // ������, �� ������� ������ �������
    [SerializeField] private float followSpeed = 5f;

    private void Start()
    {
        if (targetFollow != null)
        {
            transform.position = targetFollow.position;
        }

        UpdateRotation();
    }

    private void LateUpdate()
    {
        if (!isTurnOn) return;

        if (targetFollow != null)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetFollow.position,
                followSpeed * Time.deltaTime
            );
        }

        UpdateRotation();
    }

    private void UpdateRotation()
    {
        if (targetLook != null)
        {
            Vector3 direction = targetLook.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}