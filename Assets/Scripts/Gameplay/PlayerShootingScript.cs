using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingScript : MonoBehaviour
{

    [SerializeField] private GameObject prefabBullet;
    [SerializeField] private Transform firePoint;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // плоскость XZ

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 targetPoint = ray.GetPoint(enter);
            Vector3 direction = targetPoint - firePoint.position;

            GameObject bullet = Instantiate(prefabBullet, firePoint.position, Quaternion.identity);
            bullet.GetComponent<BulletBehaivourScript>().SetDirection(direction);
        }
    }
}
