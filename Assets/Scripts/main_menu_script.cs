using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;     

public class main_menu_script : MonoBehaviour
{

    public GameObject[] spaceShips;
    private GameObject ship;
    public List<GameObject> availableSpaceShips;
    public GameObject mainMenu;
    public GameObject gearMenu;
    
    public void StartGame()
    {
        foreach (Transform child in gearMenu.transform)
        {

            if (child.gameObject.activeSelf)
            {
                switch (child.name)
                {
                    case "Ship1":
                        PlayerPrefs.SetString("ChosenShip", PlayerController.SpaceshipsEnum.Player_Red.ToString() );
                        break;
                    case "Ship2":
                        PlayerPrefs.SetString("ChosenShip", PlayerController.SpaceshipsEnum.Player_Green.ToString() );
                        break;
                    case "Ship3":
                        PlayerPrefs.SetString("ChosenShip", PlayerController.SpaceshipsEnum.Player_Pink.ToString() );
                        break;
                }
            }
        }
        
        SceneManager.LoadScene(1);
    }

    public void SwitchToGearMenu()
    {
        SelectRandomSpaceShips();
        mainMenu.SetActive(false);
        gearMenu.SetActive(true);
        availableSpaceShips[0].SetActive(true);
    }

    public void SwitchSpaceShip(int index) {
        for (int i = 0; i < availableSpaceShips.Count; i++) {
            if (i == index) {
                availableSpaceShips[i].SetActive(true);
            } else {
                availableSpaceShips[i].SetActive(false);
            }
        }
    }
    
    private void SelectRandomSpaceShips() {
        Shuffle();
        availableSpaceShips.Clear();
        availableSpaceShips.AddRange(spaceShips.Take(3));
    }
    
    public void OnLeftArrowClick() {
        int currentIndex = GetCurrentSpaceShipIndex();
        
        int previousIndex = (currentIndex - 1 + spaceShips.Length) % spaceShips.Length;
        
        SwitchSpaceShip(previousIndex);
    }

    public void OnRightArrowClick() {
        int currentIndex = GetCurrentSpaceShipIndex();
        
        int nextIndex = (currentIndex + 1) % spaceShips.Length;
        
        SwitchSpaceShip(nextIndex);
    }

    private int GetCurrentSpaceShipIndex() {
        for (int i = 0; i < spaceShips.Length; i++) {
            if (spaceShips[i].activeSelf) {
                return i;
            }
        }
        
        return 0;
    }
    
    public void Shuffle() {
        for (int i = 0; i < spaceShips.Length; i++) {
            int rnd = Random.Range(0, spaceShips.Length);
            ship = spaceShips[rnd];
            spaceShips[rnd] = spaceShips[i];
            spaceShips[i] = ship;
        }
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }


}
