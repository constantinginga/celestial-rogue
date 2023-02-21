using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FaceMouseController : MonoBehaviour
{
    private InputActionAsset asset;

    void Awake()
    {
        asset = GetComponent<PlayerInput>().actions;
    }

    void OnEnable() 
    {
        asset.FindAction("Look").Enable();
        asset.FindAction("Look").performed += Look;
    }

    void Look(InputAction.CallbackContext context) 
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 playerPos = transform.position;
        Vector2 direction = mousePos - playerPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
