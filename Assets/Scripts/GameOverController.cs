using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverController : MonoBehaviour
{
    public InputActionAsset asset;

    void Start() { }

    public void ShowGameOverMenu(string message)
    {
        Time.timeScale = 0f;
        AudioManager.Instance.StopAll();
        asset.Disable();
        gameObject.SetActive(true);
        transform.Find("GameOver").GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = message;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        SceneManager.LoadScene(0);
    }
}
