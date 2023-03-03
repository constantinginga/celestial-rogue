using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingController : MonoBehaviour
{
    public InputActionAsset asset;
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float bulletSpeed = 10f;
    public int damageAmount = 1;
    public float shootDelay = 0.3f;
    private InputAction shootAction;
    private InputActionMap weapon;
    private GameObject bullet;
    private Coroutine shootingCoroutine;

    private void Awake()
    {
        weapon = asset.FindActionMap("Weapon");
        shootAction = weapon.FindAction("Shoot");
        shootAction.performed += _ => Shoot();
        shootAction.canceled += _ => StopShooting();
    }

    private void OnEnable()
    {
        shootAction.Enable();
    }

    private void OnDisable()
    {
        shootAction.Disable();
    }

    private void Shoot()
    {
        if (shootingCoroutine == null)
        {
            shootingCoroutine = StartCoroutine(StartShooting());
        }
    }


    private void StopShooting()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    private IEnumerator StartShooting()
    {
        while (true)
        {
            bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
            Destroy(bullet, 3f); // Destroy the bullet after 3 seconds
            yield return new WaitForSeconds(shootDelay);
        }
    }

    private void OnDestroy()
    {
        shootAction.performed -= _ => Shoot();
    }
}
