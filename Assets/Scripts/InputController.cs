using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    public InputActionAsset asset;
    private InputAction movement;
    public Vector2 mousePos,
        movementPos;
    public InputActionMap player;
    public GameObject EscMenu;

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
        Time.timeScale = 0f;
        asset.Disable();
        EscMenu.SetActive(!EscMenu.activeSelf);
    }

    public void backToGame()
    {
        asset.Enable();
        Time.timeScale = 1f;
    }

    public void backToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
