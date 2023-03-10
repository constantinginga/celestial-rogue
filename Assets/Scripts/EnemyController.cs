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
        Enemy_Red,
        Enemy_Blue,
        Enemy_Pink,
        Enemy_Green,
        Enemy_Grey,
        Enemy_Purple
    };

    public SpaceshipsEnum ChosenSpaceship;
    public int Health = 10;
    public AIDestinationSetter target;
    public Slider Heathbar;
    public ShootingController shootingController;
    public ParticleSystem DeathExplosion;
    public GameObject ShipWreck;
    public Texture2D texture;
    public bool isInvincible;

    void Awake()
    {
        isInvincible = false;
        target.target = GameObject.FindFirstObjectByType<PlayerController>().transform;
        Object[] data = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(texture));
        if (data != null)
        {
            foreach (Object obj in data)
            {
                if (obj.GetType() == typeof(Sprite))
                {
                    Sprite sprite = obj as Sprite;
                    if (sprite.name.Equals(ChosenSpaceship.ToString()))
                    {
                        GetComponent<SpriteRenderer>().sprite = sprite;
                        break;
                    }
                }
            }
        }
        switch (ChosenSpaceship)
        {
            case SpaceshipsEnum.Enemy_Red:
                //gameObject.AddComponent<Seeker>();
                break;
            case SpaceshipsEnum.Enemy_Blue:
                gameObject.AddComponent<EnemyBlue>();
                break;
            case SpaceshipsEnum.Enemy_Pink:

                break;
            case SpaceshipsEnum.Enemy_Green:

                break;
            case SpaceshipsEnum.Enemy_Grey:

                break;
            case SpaceshipsEnum.Enemy_Purple:
                gameObject.AddComponent<PurpleEnemyController>();
                break;
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
