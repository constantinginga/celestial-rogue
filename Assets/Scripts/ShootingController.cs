﻿using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.Gyroscope;

public class ShootingController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float bulletSpeed = 10f;

    public int damageAmount = 1;

    public float shootDelay = 0.3f;
    public LayerMask opponentLayer;
    private float shootAmount = 0;
    public int overHeatThreshold = 10;
    private bool isPlayer = false;
    private bool overHeat = false;
    public bool isShotGun = false;
    private InputActionAsset asset;
    public InputAction shootAction;
    private InputActionMap weapon;
    private Coroutine shootingCoroutine;
    private PlayerController playerController;

    private float lastShotTime = -Mathf.Infinity;

    private void Awake()
    {
        playerController = gameObject.GetComponentInParent<PlayerController>();
        if (playerController != null)
        {
            isPlayer = true;
        }

        if (transform.parent.gameObject.layer == 7)
        {
            asset = GetComponentInParent<InputController>().asset;
            weapon = asset.FindActionMap("Weapon");
            shootAction = weapon.FindAction("Shoot");
            shootAction.performed += _ => Shoot();
            shootAction.canceled += _ => StopShooting();
        }
        else
        {
            shootingPoint = transform;
        }
    }

    private void OnEnable()
    {
        if (transform.parent.gameObject.layer == 7)
        {
            shootAction.Enable();
        }
    }

    private void OnDisable()
    {
        if (transform.parent.gameObject.layer == 7)
        {
            shootAction.Disable();
        }
    }

    private void Shoot()
    {
        float currentTime = Time.time;
        if (currentTime - lastShotTime < shootDelay)
        {
            return;
        }

        lastShotTime = currentTime;

        if (shootingCoroutine == null)
        {
            shootingCoroutine = StartCoroutine(StartShooting());
        }
    }

    public void StopShooting()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }

        if (isPlayer)
        {
            StartCoroutine(coolDown());
        }
    }

    private void Update()
    {
        if (isPlayer)
        {
            playerController.SendMessage("updateOverheat", shootAmount);
        }
    }

    public IEnumerator coolDown()
    {
        if (overHeat && shootAmount < overHeatThreshold + 1)
        {
            shootAmount += overHeatThreshold / 2;
            overHeat = false;
        }

        float loopFor = shootAmount;
        for (int i = 0; i < loopFor; i++)
        {
            if (shootAmount > 0)
            {
                if (shootingCoroutine != null)
                {
                    break;
                }
                shootAmount -= 1;
            }
            yield return new WaitForSeconds(1);
        }
        StopCoroutine(coolDown());
    }

    public IEnumerator StartShooting()
    {
        while (true)
        {
            if (shootAmount >= overHeatThreshold && isPlayer)
            {
                overHeat = true;
            }
            else
            {
                if (isShotGun)
                {
                    // Diagonal bullets
                    Quaternion leftRotation = Quaternion.Euler(
                        shootingPoint.eulerAngles.x,
                        shootingPoint.eulerAngles.y,
                        shootingPoint.eulerAngles.z - 15
                    );
                    Quaternion rightRotation = Quaternion.Euler(
                        shootingPoint.eulerAngles.x,
                        shootingPoint.eulerAngles.y,
                        shootingPoint.eulerAngles.z + 15
                    );
                    ShootBullet(shootingPoint, leftRotation);
                    ShootBullet(shootingPoint, rightRotation);
                }

                ShootBullet(shootingPoint, shootingPoint.rotation);

                if (isPlayer)
                {
                    FindObjectOfType<AudioManager>().Play("Shoot");
	                shootAmount += 0.3f;
                }
            }

            yield return new WaitForSeconds(shootDelay);
        }
    }

    private void ShootBullet(Transform shootingPoint, Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, rotation);

        if (bullet.TryGetComponent<BulletController>(out BulletController bulletComponent))
        {
            bulletComponent.parentLayer = transform.parent.gameObject.layer;
            bulletComponent.damageAmount = damageAmount;
            bulletComponent.opponentLayer = opponentLayer.value;
        }

        // Calculate the direction of the bullet based on the rotation
        Vector2 direction = rotation * Vector2.up;

        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        Destroy(bullet, 3f); // Destroy the bullet after 3 seconds
    }

    private void OnDestroy()
    {
        if (transform.parent.gameObject.layer == 7)
        {
            shootAction.performed -= _ => Shoot();
        }
    }
}
