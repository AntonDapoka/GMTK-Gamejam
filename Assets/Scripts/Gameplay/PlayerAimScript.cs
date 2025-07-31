using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimScript : MonoBehaviour
{
    [SerializeField] private Transform player; 
    [SerializeField] private Transform aimTarget;

    private void Update()
    {
        Vector3 direction = aimTarget.position - player.position;
        direction.y = 0f; 

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            player.rotation = targetRotation;
        }
    }
}
