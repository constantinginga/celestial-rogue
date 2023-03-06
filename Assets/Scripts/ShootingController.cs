using System;
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
	private int shootAmount = 0;
	public int overHeatThreshold = 10;
	private bool isPlayer = false;
	private bool overHeat = false;
	private InputActionAsset asset;
    private InputAction shootAction;
    private InputActionMap weapon;
    private GameObject bullet;
    private Coroutine shootingCoroutine;
    private PlayerController playerController;

    private void Awake()
	{

		if (playerController = gameObject.GetComponentInParent<PlayerController>())
		{
			isPlayer = true;
		}
		
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
		if (overHeat)
		{
			shootAmount *= 2;
			overHeat = false;
		}
		
		int loopFor = shootAmount;
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
		        Debug.Log("OVERHEAT");
	        }
	        else
	        {
		        bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
		        if(bullet.TryGetComponent<BulletController>(out BulletController bulletComponent)){
			        bulletComponent.damageAmount = damageAmount;
			        bulletComponent.opponentLayer = opponentLayer.value;
		        }
		        bullet.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
		        Destroy(bullet, 3f); // Destroy the bullet after 3 seconds
		        
		        if (isPlayer)
		        {
			        shootAmount += 1;
		        }
		        
	        }
	        
	        yield return new WaitForSeconds(shootDelay);
        }
    }

    private void OnDestroy()
	{
		if(transform.parent.gameObject.layer == 7){
			shootAction.performed -= _ => Shoot();
		}
    }
}
