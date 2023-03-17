using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public InputActionAsset asset;

    void Start() { }

    public void ShowGameOverMenu()
    {
        Time.timeScale = 0f;
        asset.Disable();
        gameObject.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
