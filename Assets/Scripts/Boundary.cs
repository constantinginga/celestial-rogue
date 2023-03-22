using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Boundary : MonoBehaviour
{
	[SerializeField] private bool Horizontal;
	Transform player;
    
	void Start(){
		player = GameObject.FindFirstObjectByType<PlayerController>().transform;
	}
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
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
