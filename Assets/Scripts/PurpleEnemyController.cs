using System.Collections;
using UnityEngine;

public class PurpleEnemyController : MonoBehaviour
{
    public float FiringDistance = 10F;
    public float fireRate = 0.75F;
    public float dodgeCooldown = 2F;
    public float dodgeDistance = 3000F;
    public float circleColliderRadius = 1F;
    private CircleCollider2D circleCollider;
    bool canFire = true,
        canDodge = true;
    public EnemyController enemyController;
    ShootingController shootingController;
    Transform target,
        left,
        right;

    void Awake()
    {
        gameObject.AddComponent<CircleCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = circleColliderRadius;
        circleCollider.isTrigger = true;
        enemyController = GetComponent<EnemyController>();
        shootingController = enemyController.shootingController;
        target = enemyController.target.target;
        left = transform.Find("Left");
        right = transform.Find("Right");
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9 && canDodge)
        {
            // cast game object to bullet controller
            BulletController bulletController = other.gameObject.GetComponent<BulletController>();
            if (bulletController.parentLayer == 7)
            {
                // move the enemy sideways depending on its orientation
                Rigidbody2D rb = enemyController.GetComponent<Rigidbody2D>();
                Vector2 movementDirection =
                    Random.Range(0, 2) == 0
                        ? left.position - transform.position
                        : right.position - transform.position;
                rb.AddForce(movementDirection * dodgeDistance);
                // start the dodge cooldown
                StartCoroutine(startDodgeCooldown());
            }
        }
    }

    IEnumerator startDodgeCooldown()
    {
        enemyController.isInvincible = false;
        canDodge = false;
        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
        enemyController.isInvincible = true;
    }
}
