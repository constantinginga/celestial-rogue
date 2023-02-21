using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    private Rigidbody2D rb;
    private InputController input;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputController>();
    }

    void FixedUpdate() 
    {
        rb.velocity = new Vector2(input.movementPos.x, input.movementPos.y) * speed;
        
    }
}
