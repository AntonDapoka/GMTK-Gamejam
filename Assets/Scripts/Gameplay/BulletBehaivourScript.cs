using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaivourScript : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        dir.y = 0f;
        direction = dir.normalized;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
