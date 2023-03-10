using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

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
    public Slider healthBar;
    public ParticleSystem EngineEmission;
    public ParticleSystem DeathExplosion;
    public GameObject ShipWreck;
    public Texture2D texture;
    public Light2D EngineLight;
    private Rigidbody2D rb;
    private InputController input;
    public delegate void TakeDamageDelegate(int damageAmount);
    public event TakeDamageDelegate TakeDamageEvent;

    void Awake(){
        Object[] data = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(texture));
        if(data != null)
        {
            foreach (Object obj in data)
            {
                if (obj.GetType() == typeof(Sprite))
                {
                    Sprite sprite = obj as Sprite;
                    if(sprite.name.Equals(ChosenSpaceship.ToString())){
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
    }

    void FixedUpdate()
    {
        rb.AddRelativeForce(new Vector2(input.movementPos.x, input.movementPos.y) * speed);
    }

    void Update(){
        if(input.movementPos.x > 0 || input.movementPos.y > 0){
            EngineEmission.Play();
            EngineLight.enabled = true;
        }
        else{
            EngineEmission.Stop();
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
        currentHealth -= damageAmount;
        UpdateHealthBar();
    }

    private void Slowdown(int effectAmount){
        speed = effectAmount;
    }

    private void UpdateHealthBar()
    {
        healthBar.value = currentHealth;
    }

    private void Die()
    {
	    // Handle death
        Instantiate(DeathExplosion, transform.position, Quaternion.identity);
        GameObject shipwreck = Instantiate(ShipWreck, transform.position, Quaternion.identity);
        shipwreck.GetComponent<ShipWreckController>().CreateWreck(ChosenSpaceship.ToString());
	    Destroy(gameObject);
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
