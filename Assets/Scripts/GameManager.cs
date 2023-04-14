using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Player related components")]
    public GameObject Player;
    PlayerController PlayerController;
    ShootingController shootingController;

    [Header("Enemy related components")]
    public GameObject Enemy;
    public GameObject Boss;
    public LayerMask CollisionLayer;

    [Header("Level related attributes")]
    public float CurrentLevelLength;
    public int enemiesAmountThreshold;
    public float[] LevelLengths;
    public int Level;
    public Sprite[] LevelSprites;
    public GameObject Background;

    [Header("Level related components")]
    public Transform AsteroidParent;
    public Transform EnemiesParent;
    public GameObject HorizontalCollider;
    public GameObject VerticalCollider;
    List<BoxCollider2D> boundaryCols;
    EnemyController[] enemiesList;
    ShopHandler Shop;
    int currentTime;
    float converter;
    Timer timer;
    AteroidSpawner asteroidSpawner;
    bool stopped;

    void Awake()
    {
        Instantiate(Player, new Vector2(0, 0), Quaternion.identity);
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerController = Player.GetComponentInChildren<PlayerController>();
        shootingController = Player.GetComponentInChildren<ShootingController>();
        Shop = Player.GetComponentInChildren<ShopHandler>(true);
        timer = Player.GetComponentInChildren<Timer>();
        boundaryCols = new List<BoxCollider2D>();
        asteroidSpawner = GameObject.FindFirstObjectByType<AteroidSpawner>();
        boundaryCols.Add(HorizontalCollider.GetComponents<BoxCollider2D>()[0]);
        boundaryCols.Add(HorizontalCollider.GetComponents<BoxCollider2D>()[1]);
        boundaryCols.Add(VerticalCollider.GetComponents<BoxCollider2D>()[0]);
        boundaryCols.Add(VerticalCollider.GetComponents<BoxCollider2D>()[1]);
        enemiesAmountThreshold = 5;
        LevelLengths = new float[] { 60.0F, 120.0F, 180.0F, 240.0F };
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
        enemiesList = EnemiesParent.GetComponentsInChildren<EnemyController>();
        if (enemiesList.Length < enemiesAmountThreshold && !stopped)
        {
            SpawnEnemies(10);
        }
        if (!stopped)
        {
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
                shootingController.shootAction.Disable();
                OpenShop();
                //Despawn everything in the background meantime
                asteroidSpawner.Stop();
                DespawnEverything();
                Level++;
                ResetTimer();
            }
        }
    }

    void ResetTimer()
    {
        if (Level != 5)
        {
            converter = LevelLengths[Level - 1];
            currentTime = (int)LevelLengths[Level - 1];
            timer.Reset();
        }
        else
        {
            timer.UI.enabled = false;
        }
    }

    public void StartLevel()
    {
        stopped = false;
        if (Level != 5)
        {
            timer.UI.enabled = true;
	        asteroidSpawner.Begin();
        }
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
	            asteroidSpawner.Stop();
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
        shootingController.shootAction.Enable();
        Shop.gameObject.active = false;
    }

    void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject enemy = Instantiate(
                Enemy,
                GetRandomPositionOnBoundaryOfMap(Random.Range(0, 4)),
                Quaternion.identity
            );
            enemy
                .GetComponentInChildren<EnemyController>()
                .CreateEnemySpaceShip((EnemyController.SpaceshipsEnum)Random.Range(0, 5));
            enemy.transform.SetParent(EnemiesParent);
        }
    }

    Vector2 GetRandomPositionOnBoundaryOfMap(int index)
    {
        BoxCollider2D collider = boundaryCols[index];
        while (true)
        {
	        GameObject tmp = new GameObject();
	        tmp.name = "Enemy Spawn";
            tmp.transform.position = new Vector3(
                Random.Range(collider.bounds.min.x, collider.bounds.max.x),
                Random.Range(collider.bounds.min.y, collider.bounds.max.y),
                0
            );
            tmp.AddComponent<CircleCollider2D>();
            tmp.GetComponent<CircleCollider2D>().isTrigger = true;
            tmp.GetComponent<CircleCollider2D>().radius = 0.235F;
	        CircleCollider2D col = tmp.GetComponent<CircleCollider2D>();
	        Vector2 result = tmp.transform.position;
	        Destroy(tmp);
            if (!col.IsTouchingLayers(CollisionLayer))
            {
                return result;
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
