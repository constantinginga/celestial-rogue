using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;     

public class main_menu_script : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1); //for now 
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }


}
