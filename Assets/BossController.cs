using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
	[Header("General Attributes")]
	public int Health = 10;
	public float fireRate = 0.75F;
	[Header("Components")]
	public Slider Healthbar;
	public Transform Target;
	[Header("Booleans")]
	public bool canFire;
	[Header("Phase 1")]
	public List<ShootingController> BulletSpawns;
	public Transform BulletSpawnsParent;

	void Awake(){
		canFire = true;
		Healthbar.maxValue = Health;
		Healthbar.value = Health;
	}

	void Update(){
		if(canFire){
			BulletSpawnsParent.Rotate(0,0, 11.25F);
			foreach (ShootingController item in BulletSpawns)
			{
				StartCoroutine(item.StartShooting());
				startFiringCooldDown();
			}
		}
		else{
			foreach (ShootingController item in BulletSpawns)
			{
				item.StopShooting();
			}
		}
	}
    
	public void TakeDamage(int damage)
	{
		Health -= damage;
		UpdateHealthbar();
		if (Health <= 0)
		{
			Destroy(gameObject);
		}
	}
	
	void UpdateHealthbar(){
		Healthbar.value = Health;
	}
	
	public void startFiringCooldDown()
	{
		canFire = false;
		Invoke("clearFiringCooldown", fireRate);
	}
	
	public void clearFiringCooldown(){
		canFire = true;
	}
}
