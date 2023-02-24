using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Boundary : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private bool Horizontal;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider is the player
        if (other.gameObject.CompareTag("Player"))
        {
            Vector2 playerPosition = player.position;

            if (Horizontal)
            {
                playerPosition.y = playerPosition.y - playerPosition.y * 2;
            }
            else
            {
                playerPosition.x = playerPosition.x - playerPosition.x * 2;
            }

            player.position = playerPosition;
        }
    }
    
}
