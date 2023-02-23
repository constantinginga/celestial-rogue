using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private float offSet;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerPosition.position = playerPosition.position - (playerPosition.position * 2);
            Debug.Log("playerPosition.position: " + playerPosition.position);
            Debug.Log("collided");
        }
    }

    private void Update()
    {
        Debug.Log("playerPosition.position: " + playerPosition.position);
    }
}
