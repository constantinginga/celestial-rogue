using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;
using Object = UnityEngine.Object;

public class PlayerController : MonoBehaviour
{
    public enum SpaceshipsEnum
    {
        Player_Red,
        Player_Blue,
        Player_Green,
        Player_Grey,
        Player_White,
        Player_Yellow,
        Player_Pink,
    };

    public SpaceshipsEnum ChosenSpaceship;
    public float speed = 1;
    public int maxHealth = 100;
    public int currentHealth;
    public int Money = 0;
    public Slider healthBar;
    public Slider overHeat;
    public TextMeshProUGUI credits;
    public ParticleSystem EngineEmission;
    public ParticleSystem DeathExplosion;
    public GameObject ShipWreck;
    public Texture2D texture;
    public Light2D EngineLight;
    private Rigidbody2D rb;
    public InputController input;
    public delegate void TakeDamageDelegate(int damageAmount);
    public event TakeDamageDelegate TakeDamageEvent;
    public GameOverController GameOverController;

    void Awake()
    {
        Enum.TryParse<SpaceshipsEnum>(PlayerPrefs.GetString("ChosenShip"), out ChosenSpaceship);
        this.maxHealth = Mathf.RoundToInt(PlayerPrefs.GetFloat("ShipHealth"));
        this.healthBar.maxValue = Mathf.RoundToInt(PlayerPrefs.GetFloat("ShipHealth"));
        this.speed = PlayerPrefs.GetFloat("ShipSpeed");

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
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputController>();
        currentHealth = maxHealth;
        UpdateHealthBar();
        overHeat.maxValue = 10;
    }

    void FixedUpdate()
    {
        rb.AddRelativeForce(new Vector2(input.movementPos.x, input.movementPos.y) * speed);
    }

    void Update()
    {
        if (input.movementPos.x != 0 || input.movementPos.y != 0)
        {
            EngineEmission.Play();
            if (!AudioManager.Instance.isPlayed("ShipThruster"))
            {
                AudioManager.Instance.Play("ShipThruster");
            }
            EngineLight.enabled = true;
        }
        else
        {
            EngineEmission.Stop();
            AudioManager.Instance.Stop("ShipThruster");
            EngineLight.enabled = false;
        }
    }

    private void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        AudioManager.Instance.Play("TakeDamage");
        currentHealth -= damageAmount;
        UpdateHealthBar();
    }

    private void updateOverheat(int value)
    {
        if (value > overHeat.maxValue)
        {
            overHeat.maxValue = value;
        }
        overHeat.value = value;
    }

    private void updateMoney()
    {
        credits.text = "Credits: " + Money.ToString();
    }

    private void Slowdown(int effectAmount)
    {
        speed = effectAmount;
    }

    private void UpdateHealthBar()
    {
        healthBar.value = currentHealth;
    }

    // Handle death
    private void Die()
    {
        Instantiate(DeathExplosion, transform.position, Quaternion.identity);
        GameObject shipwreck = Instantiate(ShipWreck, transform.position, Quaternion.identity);
        shipwreck.GetComponent<ShipWreckController>().CreateWreck(ChosenSpaceship.ToString());
        //Some transition?
        //SceneManager.LoadScene(2);
        GameOverController.ShowGameOverMenu("You died!");
        Destroy(gameObject);
    }

    public void UpdateShip(ShopHandler.UpgradeType type, int cost, float effectAmount)
    {
        // extra check, just in case
        if (Money < cost)
        {
            return;
        }

        switch (type)
        {
            case ShopHandler.UpgradeType.RestoreHP:
                currentHealth = maxHealth;
                UpdateHealthBar();
                break;
            case ShopHandler.UpgradeType.UpgradeHP:
                var prevMaxHealth = maxHealth;
                maxHealth = (int)(maxHealth * effectAmount);
                currentHealth += maxHealth - prevMaxHealth;
                break;
            case ShopHandler.UpgradeType.UpgradeDamage:
            //
            case ShopHandler.UpgradeType.UpgradeSpeed:
                speed *= effectAmount;
                break;
            case ShopHandler.UpgradeType.ReduceOverheat:
                updateOverheat((int)effectAmount);
                break;
        }

        Money -= cost;
        updateMoney();
    }

    private void OnEnable()
    {
        TakeDamageEvent += TakeDamage;
    }

    private void OnDisable()
    {
        TakeDamageEvent -= TakeDamage;
    }
}
