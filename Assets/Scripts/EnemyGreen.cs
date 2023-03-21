using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGreen : MonoBehaviour
{
    public int Health = 30;
    public float fireRate = 0.5F;
    public float FiringDistance = 5F;
    
    public EnemyController enemyController;
    ShootingController shootingController;
    private Transform target;
    private bool canFire = true;


    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        enemyController.Health = Health;
        enemyController.Heathbar.maxValue = Health;
        enemyController.Heathbar.value = Health;
        shootingController = enemyController.shootingController;
        shootingController.isShotGun = true;
        shootingController.damageAmount = 2;
        target = enemyController.target.target;
    }
    
    void Update()
    {
        if (target && isInFiringDistance() && canFire)
        {
            StartCoroutine(shootingController.StartShooting());
            startFiringCooldDown();
        }
        else
        {
            shootingController.StopShooting();
        }
    }
    
    bool isInFiringDistance()
    {
        return Vector2.Distance(transform.position, target.position) <= FiringDistance;
    }
    
    public void startFiringCooldDown()
    {
        canFire = false;
        Invoke("clearFiringCooldown", fireRate);
    }

    public void clearFiringCooldown()
    {
        canFire = true;
    }
}
