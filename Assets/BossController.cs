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
	public Slider Heathbar;
	public Transform Target;
	public List<ShootingController> BulletSpawns;
	public Transform BulletSpawnsParent;
	[Header("Booleans")]
	public bool canFire = true;

	void Update(){
		if(canFire){
			if(BulletSpawnsParent.rotation.z == 0){
				BulletSpawnsParent.rotation = Quaternion.Euler(0,0,22.5F);
			}
			else{
				BulletSpawnsParent.rotation = Quaternion.Euler(0,0,0);
			}
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
			Destroy(transform.parent.gameObject);
		}
	}
	
	void UpdateHealthbar(){
		Heathbar.value = Health;
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
