using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    private Vector2 mapScale;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform target;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mapScale = new Vector2(transform.localScale.x, transform.localScale.y);
        Debug.Log("Sprite scale: " + mapScale);
    }

    private void Update()
    {
        Debug.Log("Sprite scale: " + target.position);
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     // Check if the object that collided with the boundary is the player
    //     if (other.CompareTag("Player"))
    //     {
    //         // Get the position of the object that collided with the boundary
    //         Vector3 position = other.transform.position;
    //
    //         // Teleport the object to the opposite side of the map
    //         if (position.x < transform.position.x - mapScale.x / 2)
    //         {
    //             position.x = transform.position.x + mapScale.x / 2;
    //         }
    //         else if (position.x > transform.position.x + mapScale.x / 2)
    //         {
    //             position.x = transform.position.x - mapScale.x / 2;
    //         }
    //
    //         if (position.y < transform.position.y - mapScale.y / 2)
    //         {
    //             position.y = transform.position.y + mapScale.y / 2;
    //         }
    //         else if (position.y > transform.position.y + mapScale.y / 2)
    //         {
    //             position.y = transform.position.y - mapScale.y / 2;
    //         }
    //
    //         other.transform.position = position;
    //     }
    // }
}
