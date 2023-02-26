using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public InputActionAsset asset;
    private InputAction movement;
    public Vector2 mousePos, movementPos;
    public InputActionMap player;

    void Awake()
    {
        player = asset.FindActionMap("Player");
    }

    private void OnEnable() {
        movement = player.FindAction("Move");
        movement.Enable();
    }

    private void OnDisable() {
        movement.Disable();
    }

    void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
        movementPos = movement.ReadValue<Vector2>();
    }
}
