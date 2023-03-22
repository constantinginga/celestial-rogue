using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
	[Header("General Attributes")]
	public int Health;
	public int MaxHealth;
	public int CurrentPhase;
	[Header("Components")]
	public Slider Healthbar;
	[Header("Phase 1")]
	public bool canFire;
	public float fireRate = 0.75F;
	public List<ShootingController> BulletSpawns;
	public Transform BulletSpawnsParent;
	[Header("Phase 2")]
	public float AOESpawnRate = 1F;
	public int AmountOfSpawns = 5;
	public bool canSpawnAOE;
	public GameObject AOEPrefab;
	public LayerMask blockingLayer;
	[Header("Phase 3")]
	public bool canSpawnMobs;
	public float MobsSpawnRate = 20F;
	public int AmountOfMobs = 3;
	public Transform MobSpawnTransform;
	public GameObject EnemyPrefab;

	void Awake(){
		canFire = true;
		canSpawnAOE = true;
		canSpawnMobs = true;
		MaxHealth = Health;
		Healthbar.maxValue = MaxHealth;
		Healthbar.value = Health;
		CurrentPhase = 1;
	}

	void Update(){
		if(CurrentPhase >= 1){
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
		if(CurrentPhase >= 2){
			if(canSpawnAOE){
				startAOECoolDown();
				for(int i= 0; i < AmountOfSpawns; i++){
					SpawnAOE();
				}
			}
		}
		if(CurrentPhase >= 3){
			if(canSpawnMobs){
				startMobSpawnCoolDown();
				for(int i= 0; i < AmountOfMobs; i++){
					SpawnMob();
				}
			}
		}
	}
    
	public void TakeDamage(int damage)
	{
		Health -= damage;
		UpdateHealthbar();
		if (Health <= 0)
		{
			//Spawn win popup
			GameObject.FindFirstObjectByType<GameOverController>().ShowGameOverMenu();
			Destroy(gameObject);
		}
		else if(Health <= MaxHealth/3){
			CurrentPhase = 3;
		}
		else if(Health <= (MaxHealth/3) * 2){
			CurrentPhase = 2;
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

	public void startAOECoolDown(){
		canSpawnAOE = false;
		Invoke("clearAOECooldown", AOESpawnRate);
	}

	public void clearAOECooldown(){
		canSpawnAOE = true;
	}

	public void startMobSpawnCoolDown(){
		canSpawnMobs = false;
		Invoke("clearMobSpawnCooldown", AOESpawnRate);
	}

	public void clearMobSpawnCooldown(){
		canSpawnMobs = true;
	}

	void SpawnAOE(){
		Vector3 position = GetRandomAOESpawnPosition();
		Instantiate(AOEPrefab, position, Quaternion.identity);
	}

	void SpawnMob(){
		Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
	}

	Vector3 GetRandomAOESpawnPosition(){
		while(true){
			GameObject tmp = new GameObject();
			tmp.transform.position = new Vector3(Random.Range(-50F, 50F), Random.Range(-20F, 20F), 0);
			tmp.AddComponent<CircleCollider2D>();
			tmp.GetComponent<CircleCollider2D>().isTrigger = true;
			CircleCollider2D col = tmp.GetComponent<CircleCollider2D>();
			if (!col.IsTouchingLayers(blockingLayer))
			{
				return tmp.transform.position;
			}
		}
	}
}
