using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;
    public Slider overHeat;
    public TextMeshProUGUI credits;
    public ParticleSystem EngineEmission;
    public Light2D EngineLight;
    private Rigidbody2D rb;
    private InputController input;
    public delegate void TakeDamageDelegate(int damageAmount);
    public event TakeDamageDelegate TakeDamageEvent;

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

    private void updateOverheat(int value)
    {
        overHeat.value = value;
    }
    
    private void UpdateHealthBar()
    {
        healthBar.value = currentHealth;
    }

    private void Die()
    {
	    // Handle death
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
