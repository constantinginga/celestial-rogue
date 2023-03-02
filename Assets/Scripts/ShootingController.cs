using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float bulletSpeed = 10f;
    private InputAction shootAction;

    private void Awake()
    {
        shootAction = new InputAction("Shoot", binding: "<Mouse>/leftButton");
        shootAction.performed += _ => Shoot();
    }

    private void OnEnable()
    {
        shootAction.Enable();
    }

    private void OnDisable()
    {
        shootAction.Disable();
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
        Destroy(bullet, 3f); // Destroy the bullet after 3 seconds
    }
}
