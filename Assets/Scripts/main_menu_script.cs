using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class main_menu_script : MonoBehaviour
{

    public GameObject[] spaceShips;
    private GameObject ship;
    public List<GameObject> availableSpaceShips;
    public GameObject mainMenu;
    public GameObject gearMenu;

    
    public void StartGame()
    {
        AudioManager.Instance.StopAll();
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
                    case "Ship4":
                        PlayerPrefs.SetString("ChosenShip", PlayerController.SpaceshipsEnum.Player_Blue.ToString() );
                        break;
                    case "Ship5":
                        PlayerPrefs.SetString("ChosenShip", PlayerController.SpaceshipsEnum.Player_Grey.ToString() );
                        break;
                    case "Ship6":
                        PlayerPrefs.SetString("ChosenShip", PlayerController.SpaceshipsEnum.Player_White.ToString() );
                        break;
                    case "Ship7":
                        PlayerPrefs.SetString("ChosenShip", PlayerController.SpaceshipsEnum.Player_Yellow.ToString() );
                        break;
                }
            }
        }
        
        SceneManager.LoadScene(1);
    }

    private void Start()
    {
        AudioManager.Instance.Play("MenuSong");
    }

    public void SliderSetVolume(float volume)
    {
        AudioManager.Instance.setVolume(volume);
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

        if (currentIndex == 0)
        {
            currentIndex = availableSpaceShips.Count() - 1;
        }
        else
        {
            currentIndex -= 1;
        }
        
        SwitchSpaceShip(currentIndex);
    }

    public void OnRightArrowClick() {
        int currentIndex = GetCurrentSpaceShipIndex();
  
        if (currentIndex == availableSpaceShips.Count() - 1 )
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex += 1;
        }
        
        SwitchSpaceShip(currentIndex);
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
