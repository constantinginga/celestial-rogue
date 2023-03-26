using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

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
    public float CurrentLevelLength;
    public float[] LevelLengths;
    public int Level;
    public Sprite[] LevelSprites;
    public GameObject Background;

    [Header("Level related components")]
    public Transform AsteroidParent;
    public Transform EnemiesParent;
    ShopHandler Shop;
    int currentTime;
    float converter;
    Timer timer;
    AteroidSpawner asteroidSpawner;
    bool stopped;
    private GameOverController gameOverMenu;


    void Awake()
    {
        Instantiate(Player, new Vector2(0, 0), Quaternion.identity);
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerController = Player.GetComponentInChildren<PlayerController>();
        Shop = Player.GetComponentInChildren<ShopHandler>(true);
        timer = Player.GetComponentInChildren<Timer>();
        asteroidSpawner = GameObject.FindFirstObjectByType<AteroidSpawner>();
        LevelLengths = new float[] {60.0F, 120.0F, 180.0F, 240.0F};
        currentTime = (int)LevelLengths[0];
        converter = LevelLengths[0];
        Level = 1;
        stopped = false;
 
        timer.Start();
        StartLevel();
    }

    // Update is called once per frame
    void Update()
	{
		if(!stopped){
			converter -= Time.deltaTime;
			currentTime = (int)converter;
		}
	    if (currentTime <= 0 && PlayerController.currentHealth > 0 && !stopped)
        {
            if (Level != 5)
            {
                //Some transition maybe?
                stopped = true;
                timer.stopped = true;
                OpenShop();
                //Despawn everything in the background meantime
                asteroidSpawner.Stop();
                DespawnEverything();
                Level++;
	            ResetTimer();
            }
        }
	}
    
	void ResetTimer(){
        if(Level != 5){
            converter = LevelLengths[Level - 1];
            currentTime = (int)LevelLengths[Level - 1];
            timer.Reset();
        }
        else{
            timer.UI.enabled = false;
        }
	}

    public void StartLevel()
    {
	    stopped = false;
	    if (Level != 5)
	    {
		    timer.UI.enabled = true;
	    }
        asteroidSpawner.Begin();
        Background.GetComponent<SpriteRenderer>().sprite = LevelSprites[Random.Range(0, 2)];
        AudioManager.Instance.Play("GameplaySong");
        
        switch (Level)
        {
            case 1:
	            CurrentLevelLength = LevelLengths[0];
                SpawnEnemies(10);
                break;
            case 2:
	            CurrentLevelLength = LevelLengths[1];
                SpawnEnemies(20);
                break;
            case 3:
	            CurrentLevelLength = LevelLengths[2];
                SpawnEnemies(30);
                break;
            case 4:
	            CurrentLevelLength = LevelLengths[3];
                SpawnEnemies(40);
                break;
            case 5:
                AudioManager.Instance.Stop("GameplaySong");
                AudioManager.Instance.Play("BossScene");
                CurrentLevelLength = Mathf.Infinity;
                PlayerController.transform.position = new Vector2(30, -10);
                SpawnBoss();
                break;
            default:
                break;
        }
	    CloseShop();
		timer.Start();
    }

    void OpenShop()
    {
        Shop.gameObject.active = true;
        AudioManager.Instance.Stop("GameplaySong");
    }

    void CloseShop()
    {
        Shop.gameObject.active = false;
    }

    void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject enemy = Instantiate(Enemy, GetRandomPositionOnMap(), Quaternion.identity);
            enemy
                .GetComponentInChildren<EnemyController>()
                .CreateEnemySpaceShip((EnemyController.SpaceshipsEnum)Random.RandomRange(0, 6));
            enemy.transform.SetParent(EnemiesParent);
        }
    }

    Vector2 GetRandomPositionOnMap()
    {
        while (true)
        {
            GameObject tmp = new GameObject();
            tmp.transform.position = new Vector3(
                Random.Range(-50F, 50F),
                Random.Range(-20F, 20F),
                0
            );
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

    void SpawnBoss()
    {
        GameObject boss = Instantiate(Boss, new Vector3(0, 0, 0), Quaternion.identity);
    }

    void DespawnEverything()
    {
        foreach (
            AsteroidScript asteroid in AsteroidParent.GetComponentsInChildren<AsteroidScript>()
        )
        {
            Destroy(asteroid.gameObject);
        }
        foreach (EnemyController enemy in EnemiesParent.GetComponentsInChildren<EnemyController>())
        {
            Destroy(enemy.transform.parent.gameObject);
        }
    }
}
