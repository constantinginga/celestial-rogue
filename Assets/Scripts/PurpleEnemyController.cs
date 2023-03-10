using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleEnemyController : MonoBehaviour
{
    public float FiringDistance = 10F;
    public float fireRate = 0.75F;
    public float dodgeCooldown = 2F;
    public float dodgeDistance = 2000F;
    bool canFire = true,
        canDodge = true;
    public EnemyController enemyController;
    ShootingController shootingController,
        playerShootingController;

    Transform target;
    Transform left,
        right;

    void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        playerShootingController = GameObject
            .FindObjectOfType<InputController>()
            .GetComponentInChildren<ShootingController>();
        shootingController = enemyController.shootingController;
        target = enemyController.target.target;
        left = transform.Find("Left");
        right = transform.Find("Right");
    }

    void Update()
    {
        if (canDodge && playerShootingController.shootAction.inProgress)
        {
            Dodge();
        }
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

    void Dodge()
    {
        StartCoroutine(startDodgeCooldown());
        // move the enemy sideways depending on its orientation
        Rigidbody2D rb = enemyController.GetComponent<Rigidbody2D>();
        Vector2 movementDirection =
            Random.Range(0, 1) == 0
                ? left.position - transform.position
                : right.position - transform.position;
        rb.AddForce(movementDirection * dodgeDistance);
    }

    IEnumerator startDodgeCooldown()
    {
        canDodge = false;
        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }
}
