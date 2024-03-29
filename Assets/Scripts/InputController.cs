﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class InputController : MonoBehaviour
{
    public InputActionAsset asset;
    private InputAction movement;
    public Vector2 mousePos,
        movementPos;
    public InputActionMap player;
    public GameObject EscMenu;
    public Slider volume;

    void Awake()
    {
        player = asset.FindActionMap("Player");
    }

    private void OnEnable()
    {
        movement = player.FindAction("Move");
        movement.Enable();
        asset.FindAction("Open").Enable();
        asset.FindAction("Open").performed += Open;
    }

    private void OnDisable()
    {
        movement.Disable();
    }

    void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
        movementPos = movement.ReadValue<Vector2>();
    }

    void Open(InputAction.CallbackContext context)
    {
        if (!EscMenu.activeSelf)
        {
            Time.timeScale = 0f;
            asset.FindActionMap("Player").Disable();
            asset.FindActionMap("Weapon").Disable();
            AudioManager.Instance.Pause("GameplaySong");
            EscMenu.SetActive(true);
        }
        else
        {
            EscMenu.SetActive(false);
            backToGame();
        }
    }

    public void backToGame()
    {
        AudioManager.Instance.Resume("GameplaySong");
        asset.FindActionMap("Player").Enable();
        asset.FindActionMap("Weapon").Enable();
        Time.timeScale = 1f;
    }

    public void updateVolume(float volume)
    {
        AudioManager.Instance.setVolume(volume);
    }

    public void backToMainMenu()
	{
        AudioManager.Instance.Stop("GameplaySong");
		Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
