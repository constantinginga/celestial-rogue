using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class AsteroidScript : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    
    [SerializeField] public float asteroidHp;
    [SerializeField] public float size = 1.0f;
    [SerializeField] public float minSize = 0.5f;
    [SerializeField] public float maxSize = 2f;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float maxLifetime = 20.0f;
    
    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;

        rigidBody2D.mass = size * 2;
        asteroidHp = rigidBody2D.mass;
    }

    public void setTrajectory(Vector2 direction)
    {
        rigidBody2D.AddForce(direction * this.speed);
        
        Destroy(this.gameObject, this.maxLifetime);
    }
    
	private void TakeDamage(int damage){
        asteroidHp -= damage;

        if (asteroidHp <= 0)
        {
            if ((this.size / 2) >= this.minSize)
            {
                createSplitAsteroid();
                createSplitAsteroid();
            }
            
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            collision.gameObject.SendMessage("TakeDamage", 10 + speed);
        }
    }
    
    private void createSplitAsteroid()
    {
        AsteroidScript half = Instantiate(this, transform.position, this.transform.rotation);
        half.size = this.size / 2;
        half.setTrajectory(Random.insideUnitCircle.normalized * this.speed * 5f);
    }
}
