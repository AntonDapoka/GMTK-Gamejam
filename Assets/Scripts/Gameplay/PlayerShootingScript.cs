using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingScript : MonoBehaviour
{

    [SerializeField] private GameObject prefabBullet;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform aimTarget; // цель, в которую стреляем
    [SerializeField] private float shootInterval = 0.2f; 


    private bool isShooting = false;

    /*private void Update()
    {
        if (isShooting == false && Input.GetMouseButtonDown(0))
        {
            isShooting = true;
            StartCoroutine(ShootingLoop());
        }
        else if (isShooting == true && Input.GetMouseButtonDown(0))
        {
            isShooting = false;
        }
    }*/

    private IEnumerator ShootingLoop()
    {
        while (isShooting)
        {
            ShootBullet();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    public void ShootBullet()
    {
        Vector3 direction = (aimTarget.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(prefabBullet, firePoint.position, Quaternion.identity);

        BulletBehaivourScript bulletScript = bullet.GetComponent<BulletBehaivourScript>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }
        else
        {
            Debug.Log("Problemssss");
        }
    }
}