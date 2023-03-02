using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingController : MonoBehaviour
{
    public InputActionAsset asset;
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float bulletSpeed = 10f;
    public int damageAmount = 1;
    private InputAction shootAction;
    private InputActionMap weapon;
    private GameObject bullet;

    private void Awake()
    {
        weapon = asset.FindActionMap("Weapon");
        shootAction = weapon.FindAction("Shoot");
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
        bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
        Destroy(bullet, 3f); // Destroy the bullet after 3 seconds
    }
    private void OnDestroy()
    {
        shootAction.performed -= _ => Shoot();
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //     {
    //         var enemy = collision.gameObject.GetComponent<EnemyController>();
    //         if (enemy != null)
    //         {
    //             enemy.TakeDamage(damageAmount);
    //         }
    //         Destroy(bullet);
    //     }
    // }
}
