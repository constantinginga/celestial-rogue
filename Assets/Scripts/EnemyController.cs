using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine; 
using UnityEngine.UI;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
	public int Health = 10;
	public float FiringDistance = 10F;
	public float fireRate = 0.75F;
	public AIDestinationSetter target;
	public Slider Heathbar;
	public ShootingController shootingController;
	bool canFire = true;

    void Update()
    {
	    if(target.target && isInFiringDistance() && canFire){
	    	StartCoroutine(shootingController.StartShooting());
	    	startFiringCooldDown();
		}
	    else{
	    	shootingController.StopShooting();
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
    
	bool isInFiringDistance(){
		return Vector2.Distance(transform.position, target.target.position) <= FiringDistance;
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
