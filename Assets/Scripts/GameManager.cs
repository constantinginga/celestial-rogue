using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	[Header("Player related components")]
	public GameObject Player;
	PlayerController PlayerController;
	[Header("Enemy related components")]
	public GameObject Enemy;
	public GameObject Boss;
	public LayerMask CollisionLayer;
	[Header("Level related attributes")]
	public float LevelLength;
	public int Level;
	[Header("Level related components")]
	public Transform AsteroidParent;
	public Transform EnemiesParent;
	ShopHandler Shop;
	int currentTime = 0;
	float converter = 0.0F;
	Timer timer;
	AteroidSpawner asteroidSpawner;

	void Awake(){
		Instantiate(Player, new Vector2(0,0), Quaternion.identity);
		PlayerController = Player.GetComponentInChildren<PlayerController>();
		Shop = Player.GetComponentInChildren<ShopHandler>(true);
		timer = Player.GetComponentInChildren<Timer>();
		asteroidSpawner = GameObject.FindFirstObjectByType<AteroidSpawner>();
		Level = 1;
		StartLevel();
	}

    // Update is called once per frame
    void Update()
	{
		converter += Time.deltaTime;
		currentTime = (int)converter;
		if(currentTime == LevelLength && PlayerController.currentHealth > 0){
			//Some transition maybe?
			//Open shop
			print("Success");
			Shop.gameObject.SetActive(true);
			//Despawn everything in the background meantime
			asteroidSpawner.Stop();
			DespawnEverything();
			currentTime = 0;
			timer.currentTime = 0;
			timer.stopped = true;
			Level++;
		}
	}
	
	void StartLevel(){
		asteroidSpawner.Begin();
		switch (Level)
		{
		case 1:
			LevelLength = 12;
			SpawnEnemies(10);
			break;
		case 2:
			LevelLength = 180;
			SpawnEnemies(20);
			break;
		case 3:
			LevelLength = 240;
			SpawnEnemies(30);
			break;	
		case 4:
			LevelLength = 300;
			SpawnEnemies(40);
			break;
		case 5:
			LevelLength = Mathf.Infinity;
			PlayerController.transform.position = new Vector2(30, -10);
			SpawnBoss();
			break;
		default:
			break;
		}
	}
    
	void SpawnEnemies(int amount){
		for (int i = 0; i < amount; i++) {
			GameObject enemy = Instantiate(Enemy, GetRandomPositionOnMap(), Quaternion.identity);
			enemy.GetComponentInChildren<EnemyController>().CreateEnemySpaceShip((EnemyController.SpaceshipsEnum)Random.RandomRange(0,5));
			enemy.transform.SetParent(EnemiesParent);
		}
	}
	
	Vector2 GetRandomPositionOnMap(){
		while(true){
			GameObject tmp = new GameObject();
			tmp.transform.position = new Vector3(Random.Range(-50F, 50F), Random.Range(-20F, 20F), 0);
			tmp.AddComponent<CircleCollider2D>();
			tmp.GetComponent<CircleCollider2D>().isTrigger = true;
			tmp.GetComponent<CircleCollider2D>().radius = 0.235F;
			CircleCollider2D col = tmp.GetComponent<CircleCollider2D>();
			if (!col.IsTouchingLayers(CollisionLayer))
			{
				return tmp.transform.position;
			}
		}
	}
	
	void SpawnBoss(){
		GameObject boss = Instantiate(Boss, new Vector3(0,0,0), Quaternion.identity);
	}
	
	void DespawnEverything(){
		foreach (AsteroidScript asteroid in AsteroidParent.GetComponentsInChildren<AsteroidScript>())
		{
			Destroy(asteroid.gameObject);
		}
		foreach (EnemyController enemy in EnemiesParent.GetComponentsInChildren<EnemyController>())
		{
			Destroy(enemy.gameObject);
		}
	}
}
