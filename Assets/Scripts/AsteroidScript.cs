using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class AsteroidScript : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;

    [SerializeField] public float size = 1.0f;
    [SerializeField] public float minSize = 0.5f;
    [SerializeField] public float maxSize = 1.5f;
    [SerializeField] private float speed = 50.0f;
    [SerializeField] private float maxLifetime = 30.0f;
    
    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;

        rigidBody2D.mass = size;
    }

    public void setTrajectory(Vector2 direction)
    {
        rigidBody2D.AddForce(direction * this.speed);
        
        Destroy(this.gameObject, this.maxLifetime);
    }
}
