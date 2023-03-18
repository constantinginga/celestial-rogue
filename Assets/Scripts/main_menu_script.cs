using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;     

public class main_menu_script : MonoBehaviour
{
    [SerializeField] private Transform gearMenu;
    
    
    public void StartGame()
    {

        foreach (Transform child in gearMenu)
        {

            if (child.gameObject.activeSelf)
            {
                switch (child.name)
                {
                    case "Ship1":
                        PlayerPrefs.SetString("ChosenShip", EnemyController.SpaceshipsEnum.Player_Red.ToString() );
                        break;
                    case "Ship2":
                        PlayerPrefs.SetString("ChosenShip", EnemyController.SpaceshipsEnum.Player_Green.ToString() );
                        break;
                    case "Ship3":
                        PlayerPrefs.SetString("ChosenShip", EnemyController.SpaceshipsEnum.Player_Pink.ToString() );
                        break;
                }
            }
        }
        
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }


}
