using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float bulletSpeed = 10f;
    public int damageAmount = 1;
	public float shootDelay = 0.3f;
	public LayerMask opponentLayer;
	private InputActionAsset asset;
    private InputAction shootAction;
    private InputActionMap weapon;
    private GameObject bullet;
    private Coroutine shootingCoroutine;
    private float lastShotTime = -Mathf.Infinity; 

    private void Awake()
	{
		if(transform.parent.gameObject.layer == 7){
			asset = GetComponentInParent<InputController>().asset;
			weapon = asset.FindActionMap("Weapon");
			shootAction = weapon.FindAction("Shoot");
			shootAction.performed += _ => Shoot();
			shootAction.canceled += _ => StopShooting();
		}
		else{
			shootingPoint = transform;
		}
    }

    private void OnEnable()
	{
		if(transform.parent.gameObject.layer == 7){
			shootAction.Enable();
		}
    }

    private void OnDisable()
	{
		if(transform.parent.gameObject.layer == 7){
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
    }

	public IEnumerator StartShooting()
    {
        while (true)
        {
            bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
	        if(bullet.TryGetComponent<BulletController>(out BulletController bulletComponent)){
	        	bulletComponent.damageAmount = damageAmount;
	        	bulletComponent.opponentLayer = opponentLayer.value;
	        }
	        bullet.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
            Destroy(bullet, 3f); // Destroy the bullet after 3 seconds
            yield return new WaitForSeconds(shootDelay);
        }

        shootingCoroutine = null;
    }

    private void OnDestroy()
	{
		if(transform.parent.gameObject.layer == 7){
			shootAction.performed -= _ => Shoot();
		}
    }
}
