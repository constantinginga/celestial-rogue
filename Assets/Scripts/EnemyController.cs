using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public enum SpaceshipsEnum
    {
        Enemy_Blue,
        Enemy_Pink,
        Enemy_Green,
        Enemy_Grey,
        Enemy_Purple
    };

	public SpaceshipsEnum ChosenSpaceship;
	public List<Sprite> Sprites = new List<Sprite>();
    public int Health = 10;
    public int Reward = 100;
    public AIDestinationSetter target;
    public Slider Heathbar;
    public ShootingController shootingController;
    public ParticleSystem DeathExplosion;
    public GameObject ShipWreck;
    public Texture2D texture;
    public bool isInvincible;
    PlayerController Player;

    void Awake()
    {
        isInvincible = false;
        Player = GameObject.FindFirstObjectByType<PlayerController>();
        target.target = Player.transform;
    }

    public void CreateEnemySpaceShip(SpaceshipsEnum chosenSpaceship)
    {
        switch (chosenSpaceship)
        {
            case SpaceshipsEnum.Enemy_Blue:
                gameObject.AddComponent<EnemyBlue>();
                break;
            case SpaceshipsEnum.Enemy_Pink:
                gameObject.AddComponent<EnemyPink>();
                break;
            case SpaceshipsEnum.Enemy_Green:
                gameObject.AddComponent<EnemyGreen>();
                break;
            case SpaceshipsEnum.Enemy_Grey:
                gameObject.AddComponent<EnemyGrey>();
                break;
            case SpaceshipsEnum.Enemy_Purple:
                gameObject.AddComponent<PurpleEnemyController>();
                break;
        }
	    ChosenSpaceship = chosenSpaceship;
	    foreach (Sprite sprite in Sprites)
	    {
		    if (sprite.name.Equals(ChosenSpaceship.ToString()))
		    {
			    GetComponent<SpriteRenderer>().sprite = sprite;
			    break;
		    }
	    }
    }

    void Update() { }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            if (Health <= 0)
            {
                Instantiate(DeathExplosion, transform.position, Quaternion.identity);
                GameObject shipwreck = Instantiate(
                    ShipWreck,
                    transform.position,
                    Quaternion.identity
                );
                shipwreck
                    .GetComponent<ShipWreckController>()
                    .CreateWreck(ChosenSpaceship.ToString());
                Player.Money += Reward;
                Player.SendMessage("updateMoney");
                Destroy(transform.parent.gameObject);
            }
            Health -= damage;
            UpdateHealthbar();
        }
    }

    void UpdateHealthbar()
    {
        Heathbar.value = Health;
    }
}
